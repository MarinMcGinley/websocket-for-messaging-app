using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Specifications
{
    public class FindFriendSpecification : BaseSpecification<Friend>
    {
        public FindFriendSpecification(int userId, int friendId) : 
            base(f => f.FriendUserId == friendId && f.UserId == userId)
        {
            AddInclude(friend => friend.FriendUser);
            AddInclude(friend => friend.User);
        }
    }
}