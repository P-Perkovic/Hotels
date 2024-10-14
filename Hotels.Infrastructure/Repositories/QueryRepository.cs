using Hotels.Domain.Extensions;
using Hotels.Domain.Interfaces;
using Hotels.Domain.Models;
using Hotels.Domain.Queries;
using Hotels.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hotels.Infrastructure.Repositories
{
    public abstract class QueryRepository<TEntity> : IQueryRepository<TEntity> where TEntity : Entity
    {
        protected readonly HotelsDbContext Db;
        protected readonly DbSet<TEntity> DbSet;

        protected QueryRepository(HotelsDbContext db)
        {
            Db = db;
            DbSet = db.Set<TEntity>();
        }

        public virtual async Task<TEntity> GetById(int id, CancellationToken cancellationToken)
        {
            return await DbSet.FindAsync(new object[] { id }, cancellationToken);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll(PageQuery pageQuery, CancellationToken cancellationToken)
        {
            return await DbSet.AsNoTracking()
                .OrderBy(e => e.Id)
                .Page(pageQuery)
                .ToListAsync(cancellationToken);
        }
    }
}
