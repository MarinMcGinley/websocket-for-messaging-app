using Core.Entities;

namespace Core.Interfaces
{
    public interface IMessageRepository
    {
         Task<IReadOnlyList<Message>> GetMessagesBetweenFriendsAsync(int userId, int friendId);
    }
}