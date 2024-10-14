using AutoMapper;
using FluentValidation;
using Hotels.Commands;
using Hotels.Configuration;
using Hotels.Controllers;
using Hotels.Domain.Interfaces;
using Hotels.Domain.Models;
using Hotels.Domain.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Hotels.Tests
{
    public class HotelsControlerTests
    {
        private readonly Mock<IHotelService> _hotelService;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<HotelsController>> _logger;
        private readonly IValidator<HotelCommand> _validator;

        public HotelsControlerTests()
        {
            _hotelService = new Mock<IHotelService>();
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });
            _mapper = mapperConfig.CreateMapper();
            _logger = new Mock<ILogger<HotelsController>>();
            _validator = new HotelCommandValidator();
        }

        [Fact]
        public async Task Get_ReturnsANotFound()
        {
            // Arrange
            var id = 1;
            _hotelService.Setup(s => s.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((Hotel)null);
            var controller = new HotelsController(_hotelService.Object, _logger.Object, _mapper, null);

            // Act
            var response = await controller.Get(id, new CancellationToken());

            // Assert
            Assert.IsType<NotFoundResult>(response);
        }

        [Fact]
        public async Task Get_ReturnsAHotel()
        {
            // Arrange
            var id = 1;
            _hotelService.Setup(s => s.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Hotel());
            var controller = new HotelsController(_hotelService.Object, _logger.Object, _mapper, null);

            // Act
            var response = await controller.Get(id, new CancellationToken());

            // Assert
            var result = Assert.IsType<OkObjectResult>(response);
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task GetHotels_PageSize_ReturnsAList()
        {
            // Arrange
            var pageQuery = new PageQuery
            {
                Page = 1,
                PageSize = 5
            };
            var controller = new HotelsController(_hotelService.Object, _logger.Object, _mapper, null);

            // Act
            var response = await controller.GetHotels(pageQuery, new CancellationToken());

            // Assert
            var result = Assert.IsType<OkObjectResult>(response);
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task GetHotels_PageSizeZero_ReturnsAEmptyList()
        {
            // Arrange
            var pageQuery = new PageQuery
            {
                Page = 1,
                PageSize = 5
            };
            _hotelService.Setup(s => s.GetAll(It.IsAny<PageQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<Hotel>());
            var controller = new HotelsController(_hotelService.Object, _logger.Object, _mapper, null);

            // Act
            var response = await controller.GetHotels(pageQuery, new CancellationToken());

            // Assert
            Assert.IsType<OkResult>(response);
        }

        [Fact]
        public async Task SearchByLocation_PageSize_ReturnsAList()
        {
            // Arrange
            var locationQuery = new LocationQuery
            {
                Distance = 10,
                Longitude = 16,
                Latitude = 43,
                Page = 1,
                PageSize = 2
            };

            _hotelService.Setup(s => s.SearchByLocation(It.IsAny<LocationQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<Hotel>());
            var controller = new HotelsController(_hotelService.Object, _logger.Object, _mapper, null);

            // Act
            var response = await controller.Search(locationQuery, new CancellationToken());

            // Assert
            var result = Assert.IsType<OkObjectResult>(response);
            Assert.NotNull(result.Value);
        }
        
        [Theory]
        [InlineData(-1, 16, 43, 5)]
        [InlineData(10, 1116, 43, 5)]
        [InlineData(10, 16, 433, 5)]
        [InlineData(10, 16, 43, 0)]
        public async Task SearchByLocation_ReturnsAEmptyList(int distance, double longitude, double latitude, int pageSize)
        {
            // Arrange
            var locationQuery = new LocationQuery
            {
                Distance = distance,
                Longitude = longitude,
                Latitude = latitude,
                Page = 0,
                PageSize = pageSize
            };
            var controller = new HotelsController(_hotelService.Object, _logger.Object, _mapper, null);

            // Act
            var response = await controller.Search(locationQuery, new CancellationToken());

            // Assert
            Assert.IsType<OkResult>(response);
        }

        [Fact]
        public async Task Create_ReturnsAHotel()
        {
            // Arrange
            var command = new HotelCommand
            {
                Name = "Sunset",
                Price = 150.00m,
                Longitude = 16,
                Latitude =  43
            };

            _hotelService.Setup(s => s.Add(It.IsAny<Hotel>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Hotel());
            var controller = new HotelsController(_hotelService.Object, _logger.Object, _mapper, _validator);

            // Act
            var response = await controller.Create(command, new CancellationToken());

            // Assert
            var result = Assert.IsType<CreatedAtActionResult>(response);
            Assert.NotNull(result.Value);
        }

        [Theory]
        [InlineData("", 10, 16, 43)]
        [InlineData("Test", -10, 16, 43)]
        [InlineData("Test", 10, 1166, 43)]
        [InlineData("Test", 10, 16, 433)]
        public async Task Create_ReturnsABadRequest(string name, decimal price, double longitude, double latitude)
        {
            // Arrange
            var command = new HotelCommand
            {
                Name = name,
                Price = price,
                Longitude = longitude,
                Latitude = latitude
            };

            _hotelService.Setup(s => s.Add(It.IsAny<Hotel>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Hotel());
            var controller = new HotelsController(_hotelService.Object, _logger.Object, _mapper, _validator);

            // Act
            var response = await controller.Create(command, new CancellationToken());

            // Assert
            Assert.IsType<BadRequestObjectResult>(response);
        }
    }
}
