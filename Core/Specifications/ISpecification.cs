using System.Linq.Expressions;

namespace Core.Specifications
{
    public interface ISpecification<T>
    {
         Expression<Func<T, bool>> Criteria { get; }
         List<Expression<Func<T, object>>> Includes { get; }
         List<Expression<Func<T, object>>> OrderBys { get; }
         List<Expression<Func<T, object>>> OrderBysDesc { get; }
    }
}