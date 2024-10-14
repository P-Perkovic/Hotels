using AutoMapper;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var hotel = await _hotelService.GetById(id);
            if(hotel is null)
            {
                _logger.LogInformation($"Hotel with id {id} not found.");
                return BadRequest();
            }

            return Ok(_mapper.Map<HotelDto>(hotel));
        }

        [HttpGet]
        public async Task<IActionResult> GetPage(int page, int pageSize)
        {
            var pageQuery = new PageQuery
            {
                Page = page,
                PageSize = pageSize
            };
            var hotels = await _hotelService.GetAll(pageQuery);

            return Ok(_mapper.Map<IEnumerable<HotelDto>>(hotels));
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] LocationQuery locationQuery)
        {
            var hotels = await _hotelService.SearchByLocation(locationQuery);

            return Ok(_mapper.Map<IEnumerable<HotelDto>>(hotels));
        }

        [HttpPost]
        public async Task<IActionResult> Create(HotelCommand hotelCommand)
        {
            var validationResult = _validator.Validate(hotelCommand);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var hotel = await _hotelService.Add(_mapper.Map<Hotel>(hotelCommand), hotelCommand.Longitude, hotelCommand.Latitude);
            if (hotel is null)
            {
                _logger.LogInformation($"Hotel {hotelCommand.Name} is not added.");
                return BadRequest();
            }

            return CreatedAtAction("Create", _mapper.Map<HotelDto>(hotel));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, HotelCommand hotelCommand)
        {
            var entity = await _hotelService.GetById(id);
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

            var hotel = await _hotelService.Update(id, _mapper.Map(hotelCommand, entity), hotelCommand.Longitude, hotelCommand.Latitude);
            if(hotel is null)
            {
                _logger.LogInformation($"Update failed for hotel with id {id}.");
                return BadRequest();
            }

            return Ok(_mapper.Map<HotelDto>(hotel));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await _hotelService.Delete(id);
            if(!isDeleted)
            {
                _logger.LogInformation($"Deletion failed for hotel with id {id}.");
                return BadRequest();
            }

            return NoContent();
        }
    }
}
