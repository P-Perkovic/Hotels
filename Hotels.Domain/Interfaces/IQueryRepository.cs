using Hotels.Domain.Models;
using Hotels.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hotels.Domain.Interfaces
{
    public interface IQueryRepository<TEntity> where TEntity : Entity
    {
        Task<TEntity> GetById(int id, CancellationToken cancellationToken);
        Task<IEnumerable<TEntity>> GetAll(PageQuery pageQuery, CancellationToken cancellationToken);
    }
}
