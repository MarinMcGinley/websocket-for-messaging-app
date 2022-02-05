using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications
{
    public class FriendsWithUsersSpecification : BaseSpecification<Friend>
    {
        public FriendsWithUsersSpecification(int userId, BaseSpecParams friendsSpecParams) : base(f => f.FriendUserId == userId)
        {
            AddInclude(friend => friend.FriendUser);
            AddInclude(friend => friend.User);
            ApplyPaging(friendsSpecParams.PageSize * (friendsSpecParams.PageIndex - 1), friendsSpecParams.PageSize);
        }
    }
}