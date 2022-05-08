using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications
{
    public class UsersFromEmailSpecification : BaseSpecification<User>
    {

        public UsersFromEmailSpecification(string email, int userId) : base(user => user.Email == email && user.Id != userId)
        {
        }
    }
}