using Hotels.Domain.Interfaces;
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
        private readonly ILogger _logger;

        public HotelsController(IHotelService hotelService, ILogger logger)
        {
            _hotelService = hotelService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHotel(int id)
        {
            var hotel = await _hotelService.GetById(id);
            if(hotel is null)
            {
                return BadRequest();
            }

            return Ok(hotel);
        }
    }
}
