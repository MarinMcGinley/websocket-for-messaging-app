using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications
{
    public class UsersFromSearchStringSpecification : BaseSpecification<User>
    {
        public UsersFromSearchStringSpecification(UserSpecParams userParams, int userId) : 
        base(user => user.Id != userId && 
            (user.FirstName.ToLower().Contains(userParams.searchString.ToLower()) || 
            user.LastName.ToLower().Contains(userParams.searchString.ToLower()) ||
            (user.FirstName.ToLower() + " " + user.LastName.ToLower()).Contains(userParams.searchString.ToLower()) ||
            (user.LastName.ToLower() + " " + user.FirstName.ToLower()).Contains(userParams.searchString.ToLower()) ||
            user.Email.ToLower().Contains(userParams.searchString.ToLower())))
        {
            AddOrderBy(user => user.LastName);
            ApplyPaging(userParams.PageSize * (userParams.PageIndex - 1), userParams.PageSize);
        }
    }
}