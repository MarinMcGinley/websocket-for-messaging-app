using Core.Entities;

namespace Core.Interfaces
{
    public interface IFriendRepository
    {
         Task<IReadOnlyList<Friend>> GetFriends(int userId);
    }
}