using Hotels.Domain.Models;
using Hotels.Domain.Queries;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hotels.Domain.Interfaces
{
    public interface IHotelService
    {
        Task<Hotel> GetById(int id, CancellationToken cancellationToken);
        Task<IEnumerable<Hotel>> GetAll(PageQuery pageQuery, CancellationToken cancellationToken);
        Task<IEnumerable<Hotel>> SearchByLocation(LocationQuery locationQuery, CancellationToken cancellationToken);
        Task<Hotel> Add(Hotel hotel, double longitude, double latitude, CancellationToken cancellationToken);
        Task<Hotel> Update(Hotel hotel, double longitude, double latitude, CancellationToken cancellationToken);
        Task<bool> Delete(int id, CancellationToken cancellationToken);
    }
}
