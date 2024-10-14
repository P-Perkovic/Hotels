using Hotels.Domain.Models;
using Hotels.Infrastructure.Context;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hotels.Infrastructure.Repositories
{
    public class HotelCommandRepository : CommandRepository<Hotel>
    {
        public HotelCommandRepository(HotelsDbContext db) : base(db) { }
    }
}
