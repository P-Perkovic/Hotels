using Hotels.Domain.Models;
using Hotels.Domain.Queries;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Domain.Interfaces
{
    public interface IHotelQueryRepository : IQueryRepository<Hotel>
    {
        Task<IEnumerable<Hotel>> SearchByLocation(Point point, LocationQuery locationQuery);
    }
}
