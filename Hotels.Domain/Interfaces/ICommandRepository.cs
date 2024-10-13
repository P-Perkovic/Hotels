using Hotels.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Domain.Interfaces
{
    public interface ICommandRepository<TEntity> where TEntity : Entity
    {
        Task<TEntity> Add(TEntity entity);
        Task<TEntity> Update(TEntity entity);
        Task<bool> Remove(int id);
    }
}
