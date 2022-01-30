using Core.Entities;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec) {
            var query = inputQuery;

            if (spec.Criteria != null) {
                query = query.Where(spec.Criteria);
            }

            query = spec.OrderBys.Aggregate(query, (current, orderBy) => current.OrderBy(orderBy));
            query = spec.OrderBysDesc.Aggregate(query, (current, orderBy) => current.OrderByDescending(orderBy));
            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }
    }
}