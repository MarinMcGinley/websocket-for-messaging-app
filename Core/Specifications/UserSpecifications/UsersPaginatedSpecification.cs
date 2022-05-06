using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications
{
    public class UsersPaginatedSpecification : BaseSpecification<User>
    {
        public UsersPaginatedSpecification(BaseSpecParams userParams): base() {
            AddOrderBy(user => user.LastName);
            ApplyPaging(userParams.PageSize * (userParams.PageIndex - 1), userParams.PageSize);
        }
    }
}