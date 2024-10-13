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
        protected readonly ILogger _logger;

        protected CommandRepository(HotelsDbContext db, ILogger logger)
        {
            Db = db;
            DbSet = db.Set<TEntity>();
            _logger = logger;
        }

        public virtual async Task<TEntity> Add(TEntity entity)
        {
            try
            {
                DbSet.Add(entity);
                await Db.SaveChangesAsync();
                return entity;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error adding entity {nameof(DbSet)}");
                return null;
            }
        }

        public virtual async Task<TEntity> Update(TEntity entity)
        {
            try
            {
                DbSet.Update(entity);
                await Db.SaveChangesAsync();
                return entity;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error updating {nameof(DbSet)} with id {entity.Id}");
                return null;
            }
        }

        public virtual async Task<bool> Remove(int id)
        {
            try
            {
                var entity = await DbSet.FindAsync(id);
                if (entity != null)
                {
                    DbSet.Remove(entity);
                    return await Db.SaveChangesAsync() > 0;
                }
                else
                {
                    _logger.LogWarning($"{nameof(DbSet)} with id {id} not found for deletion");
                    return false;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error deleting {nameof(DbSet)} with id {id}");
                return false;
            }
        }
    }
}
