using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications
{
    public class UsersFromSearchStringSpecification : BaseSpecification<User>
    {
        public UsersFromSearchStringSpecification(string searchString) : 
        base(user => user.Name.ToLower().Contains(searchString.ToLower()) || 
            user.Email.ToLower().Contains(searchString.ToLower()))
        {
        }
    }
}