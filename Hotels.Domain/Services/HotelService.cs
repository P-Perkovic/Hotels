using Hotels.Domain.Interfaces;
using Hotels.Domain.Models;
using Hotels.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Hotels.Domain.Queries;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System.Linq;


namespace Hotels.Domain.Services
{
    public class HotelService : IHotelService
    {
        private static readonly int SRID = 4326;

        private readonly IHotelQueryRepository _queryRepository;
        private readonly ICommandRepository<Hotel> _commandRepository;

        public HotelService(IHotelQueryRepository queryRepository, ICommandRepository<Hotel> commandRepository)
        {
            _queryRepository = queryRepository;
            _commandRepository = commandRepository;
        }

        public async Task<Hotel> GetById(int id)
        {
            return await _queryRepository.GetById(id);
        }

        public async Task<IEnumerable<Hotel>> GetAll(PageQuery pageQuery)
        {
            return await _queryRepository.GetAll(pageQuery);
        }

        public async Task<IEnumerable<Hotel>> SearchByLocation(LocationQuery locationQuery)
        {
            var point = GetPointFromCoordinates(locationQuery.Longitude, locationQuery.Latitude);
            return await _queryRepository.SearchByLocation(point, locationQuery);
        }

        public async Task<Hotel> Add(Hotel hotel, double longitude, double latitude)
        {
            hotel.Location = GetPointFromCoordinates(longitude, latitude);

            return await _commandRepository.Add(hotel);
        }

        public async Task<Hotel> Update(Hotel hotel, double longitude, double latitude)
        {
            hotel.Location = GetPointFromCoordinates(longitude, latitude);

            return await _commandRepository.Update(hotel);
        }

        public async Task<bool> Delete(int id)
        {
            return await _commandRepository.Remove(id);
        }

        //
        // Summary:
        //     Get Point object for provided longitude and latitude.
        //
        private Point GetPointFromCoordinates(double longitude, double latitude)
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: SRID);
            return geometryFactory.CreatePoint(new Coordinate(longitude, latitude));
        }
    }
}
