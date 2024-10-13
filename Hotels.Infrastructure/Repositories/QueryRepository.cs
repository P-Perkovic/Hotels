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
using System.Threading.Tasks;

namespace Hotels.Infrastructure.Repositories
{
    public abstract class QueryRepository<TEntity> : IQueryRepository<TEntity> where TEntity : Entity
    {
        protected readonly HotelsDbContext Db;
        protected readonly DbSet<TEntity> DbSet;
        protected readonly ILogger _logger;

        protected QueryRepository(HotelsDbContext db, ILogger logger)
        {
            Db = db;
            DbSet = db.Set<TEntity>();
            _logger = logger;
        }

        public virtual async Task<TEntity> GetById(int id)
        {
            try
            {
                return await DbSet.FindAsync(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error getting {nameof(DbSet)} with id {id}");
                return null;
            }
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll(PageQuery pageQuery)
        {
            return await DbSet.AsNoTracking()
                .OrderBy(e => e.Id)
                .Page(pageQuery)
                .ToListAsync();
        }
    }
}
