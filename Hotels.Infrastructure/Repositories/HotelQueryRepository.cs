using Hotels.Domain.Extensions;
using Hotels.Domain.Interfaces;
using Hotels.Domain.Models;
using Hotels.Domain.Queries;
using Hotels.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hotels.Infrastructure.Repositories
{
    public class HotelQueryRepository : QueryRepository<Hotel>, IHotelQueryRepository
    {
        public HotelQueryRepository(HotelsDbContext db) : base(db) { }



        public async Task<IEnumerable<Hotel>> SearchByLocation(Point point, LocationQuery locationQuery, CancellationToken cancellationToken)
        {
            var query = DbSet.AsNoTracking()
                .Where(h => h.Location.Distance(point) < locationQuery.Distance)
                .OrderBy(h => h.Location.Distance(point))
                .ThenBy(h => h.Price)
                .Page(locationQuery);

            return await query.ToListAsync(cancellationToken);
        }
    }
}
