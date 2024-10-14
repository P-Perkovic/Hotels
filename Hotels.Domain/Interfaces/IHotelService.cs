using Hotels.Domain.Models;
using Hotels.Domain.Queries;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Domain.Interfaces
{
    public interface IHotelService
    {
        Task<Hotel> GetById(int id);
        Task<IEnumerable<Hotel>> GetAll(PageQuery pageQuery);
        Task<IEnumerable<Hotel>> SearchByLocation(LocationQuery locationQuery);
        Task<Hotel> Add(Hotel hotel, double longitude, double latitude);
        Task<Hotel> Update(int id, Hotel hotel, double longitude, double latitude);
        Task<bool> Delete(int id);
    }
}
