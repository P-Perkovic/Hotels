﻿using AutoMapper;
using FluentValidation;
using Hotels.Commands;
using Hotels.Domain.Interfaces;
using Hotels.Domain.Models;
using Hotels.Domain.Queries;
using Hotels.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Hotels.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelService _hotelService;
        private readonly ILogger<HotelsController> _logger;
        private readonly IMapper _mapper;
        private readonly IValidator<HotelCommand> _validator;

        public HotelsController(IHotelService hotelService, ILogger<HotelsController> logger, IMapper mapper, IValidator<HotelCommand> validator)
        {
            _hotelService = hotelService;
            _logger = logger;
            _mapper = mapper;
            _validator = validator;
        }

        //
        // Summary:
        //     Get hotel with provided id.
        //
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var hotel = await _hotelService.GetById(id, cancellationToken);
            if(hotel is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<HotelDto>(hotel));
        }

        //
        // Summary:
        //     Get hotels with pagination.
        //
        [HttpGet]
        public async Task<IActionResult> GetHotels([FromQuery] PageQuery pageQuery, CancellationToken cancellationToken)
        {
            if(!pageQuery.Validate())
            {
                return Ok();
            }

            var hotels = await _hotelService.GetAll(pageQuery, cancellationToken);

            return Ok(_mapper.Map<IEnumerable<HotelDto>>(hotels));
        }

        //
        // Summary:
        //     Search hotels by distance from current location.
        //     Pagination included.
        //     Sorting by distance from current location and price (lower first.)
        //     Distance is in meters
        //
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] LocationQuery locationQuery, CancellationToken cancellationToken)
        {
            if (!locationQuery.Validate())
            {
                return Ok();
            }

            var hotels = await _hotelService.SearchByLocation(locationQuery, cancellationToken);

            return Ok(_mapper.Map<IEnumerable<HotelDto>>(hotels));
        }

        //
        // Summary:
        //     Create hotel.
        //
        [HttpPost]
        public async Task<IActionResult> Create(HotelCommand hotelCommand, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(hotelCommand);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var hotel = await _hotelService.Add(_mapper.Map<Hotel>(hotelCommand), hotelCommand.Longitude, hotelCommand.Latitude, cancellationToken);
            if (hotel is null)
            {
                _logger.LogInformation($"Hotel {hotelCommand.Name} is not added.");
                return BadRequest();
            }

            return CreatedAtAction("Create", _mapper.Map<HotelDto>(hotel));
        }

        //
        // Summary:
        //     Update hotel with provided id.
        //
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, HotelCommand hotelCommand, CancellationToken cancellationToken)
        {
            var entity = await _hotelService.GetById(id, cancellationToken);
            if(entity is null)
            {
                _logger.LogInformation($"Hotel with id {id} doesn't exist.");
                return BadRequest();
            }

            var validationResult = _validator.Validate(hotelCommand);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var hotel = await _hotelService.Update(_mapper.Map(hotelCommand, entity), hotelCommand.Longitude, hotelCommand.Latitude, cancellationToken);
            if(hotel is null)
            {
                _logger.LogInformation($"Update failed for hotel with id {id}.");
                return BadRequest();
            }

            return Ok(_mapper.Map<HotelDto>(hotel));
        }

        //
        // Summary:
        //     Delete hotel with provided id.
        //
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var isDeleted = await _hotelService.Delete(id, cancellationToken);
            if(!isDeleted)
            {
                _logger.LogInformation($"Deletion failed for hotel with id {id}.");
                return BadRequest();
            }

            return NoContent();
        }
    }
}
