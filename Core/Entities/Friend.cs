namespace Core.Entities
{
    public class Friend : BaseEntity
    {
        public int UserId { get; set; }
        public User? User { get; set; }
        public int FriendUserId { get; set; }
        public User? FriendUser { get; set; }
    }
}