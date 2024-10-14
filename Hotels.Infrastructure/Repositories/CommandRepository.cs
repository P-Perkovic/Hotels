using Hotels.Domain.Interfaces;
using Hotels.Domain.Models;
using Hotels.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hotels.Infrastructure.Repositories
{
    public abstract class CommandRepository<TEntity> : ICommandRepository<TEntity> where TEntity : Entity
    {
        protected readonly HotelsDbContext Db;
        protected readonly DbSet<TEntity> DbSet;
        protected CommandRepository(HotelsDbContext db)
        {
            Db = db;
            DbSet = db.Set<TEntity>();
        }

        public virtual async Task<TEntity> Add(TEntity entity, CancellationToken cancellationToken)
        {
            DbSet.Add(entity);
            var result = await Db.SaveChangesAsync(cancellationToken) > 0;
            return result? entity : null;
        }

        public virtual async Task<TEntity> Update(TEntity entity, CancellationToken cancellationToken)
        {
            DbSet.Update(entity);
            var result = await Db.SaveChangesAsync(cancellationToken) > 0;
            return result ? entity : null;
        }

        public virtual async Task<bool> Remove(int id, CancellationToken cancellationToken)
        {
            var entity = await DbSet.FindAsync(new object[] { id }, cancellationToken);
            if (entity != null)
            {
                DbSet.Remove(entity);
                return await Db.SaveChangesAsync(cancellationToken) > 0;
            }
            else
            {
                return false;
            }
        }
    }
}
