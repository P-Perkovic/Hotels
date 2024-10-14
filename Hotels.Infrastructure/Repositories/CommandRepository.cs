using Hotels.Domain.Interfaces;
using Hotels.Domain.Models;
using Hotels.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
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

        public virtual async Task<TEntity> Add(TEntity entity)
        {
            DbSet.Add(entity);
            var result = await Db.SaveChangesAsync() > 0;
            return result? entity : null;
        }

        public virtual async Task<TEntity> Update(TEntity entity)
        {
            DbSet.Update(entity);
            var result = await Db.SaveChangesAsync() > 0;
            return result ? entity : null;
        }

        public virtual async Task<bool> Remove(int id)
        {
            var entity = await DbSet.FindAsync(id);
            if (entity != null)
            {
                DbSet.Remove(entity);
                return await Db.SaveChangesAsync() > 0;
            }
            else
            {
                return false;
            }
        }
    }
}
