namespace Core.Entities
{
    public class Message : BaseEntity
    {
        public string TextMessage { get; set; }
        public DateTime TimeOfMessage { get; set; }
        public int SentByUserId { get; set; }
        public User SentByUser { get; set; }
        public int SentToUserId { get; set; }
        public User SentToUser { get; set; }
    }
}