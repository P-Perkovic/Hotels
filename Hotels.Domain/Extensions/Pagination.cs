using Hotels.Domain.Models;
using Hotels.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotels.Domain.Extensions
{
    public static class Pagination
    {
        public static IQueryable<TEntity> Page<TEntity>(this IQueryable<TEntity> query, PageQuery pageQuery) where TEntity : Entity
        {
            int skip = Math.Max(pageQuery.Page * (pageQuery.Page - 1), 0);
            return query.Skip(skip).Take(pageQuery.PageSize);
        }
    }
}
