using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications
{
    public class MessagesBetweenFriendsSpecification : BaseSpecification<Message>
    {
        public MessagesBetweenFriendsSpecification(int userId, int friendId, BaseSpecParams messageParams) : 
            base(m => (m.SentByUserId == userId && m.SentToUserId == friendId) || 
                (m.SentByUserId == friendId && m.SentToUserId == userId))
        {
            AddInclude(message => message.SentByUser);
            AddInclude(message => message.SentToUser);
            AddOrderByDesc(message => message.TimeOfMessage);
            ApplyPaging(messageParams.PageSize * (messageParams.PageIndex - 1), messageParams.PageSize);
        }
    }
}