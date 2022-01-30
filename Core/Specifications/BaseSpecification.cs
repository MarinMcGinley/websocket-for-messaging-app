using System.Linq.Expressions;

namespace Core.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public BaseSpecification()
        {
        }

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        public Expression<Func<T, bool>> Criteria { get; }
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();
        public List<Expression<Func<T, object>>> OrderBys { get; } = new List<Expression<Func<T, object>>>();
        public List<Expression<Func<T, object>>> OrderBysDesc { get; } = new List<Expression<Func<T, object>>>();

        protected void AddInclude(Expression<Func<T, object>> includeExpression) {
            Includes.Add(includeExpression);
        }

        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression) {
            OrderBys.Add(orderByExpression);
        }

        protected void AddOrderByDesc(Expression<Func<T, object>> orderByExpression) {
            OrderBysDesc.Add(orderByExpression);

        }
    }
}