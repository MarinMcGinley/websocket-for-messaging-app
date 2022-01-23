using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class FriendRepository : IFriendRepository
    {
        private readonly UserContext _context;
        public FriendRepository(UserContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Friend>> GetFriends(int userId)
        {
            return await _context.Friends
                .Where(f => f.FriendUserId == userId)
                .Include(f => f.User)
                .ToListAsync();
        }
    }
}