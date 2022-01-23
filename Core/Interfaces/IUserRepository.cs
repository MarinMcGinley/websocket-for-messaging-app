using System.Collections.Generic;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(int id);
        Task<IReadOnlyList<User>> GetUsersAsync();
    }
}