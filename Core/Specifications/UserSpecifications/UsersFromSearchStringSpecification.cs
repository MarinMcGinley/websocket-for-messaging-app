using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications
{
    public class UsersFromSearchStringSpecification : BaseSpecification<User>
    {
        public UsersFromSearchStringSpecification(UserSpecParams userParams) : 
        base(user => user.Name.ToLower().Contains(userParams.searchString.ToLower()) || 
            user.Email.ToLower().Contains(userParams.searchString.ToLower()))
        {
            AddOrderBy(user => user.Name);
            ApplyPaging(userParams.PageSize * (userParams.PageIndex - 1), userParams.PageSize);
        }
    }
}