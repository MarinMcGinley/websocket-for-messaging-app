using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications
{
    public class UsersPaginatedSpecification : BaseSpecification<User>
    {
        public UsersPaginatedSpecification(UserSpecParams userParams): base() {
            AddOrderBy(user => user.Name);
            ApplyPaging(userParams.PageSize * (userParams.PageIndex - 1), userParams.PageSize);
        }
    }
}