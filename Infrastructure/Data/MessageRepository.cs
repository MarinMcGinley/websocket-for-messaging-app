using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly UserContext _context;

        public MessageRepository(UserContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Message>> GetMessagesBetweenFriendsAsync(int userId, int friendId)
        {
            return await _context.Messages
                .Where(m => (m.SentByUserId == userId && m.SentToUserId == friendId) || (m.SentByUserId == friendId && m.SentToUserId == userId))
                .Include(m => m.SentByUser)
                .Include(m => m.SentToUser)
                .OrderByDescending(m => m.TimeOfMessage)
                .ToListAsync();
        }
    }
}