namespace Core.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string TextMessage { get; set; }
        public DateTime TimeOfMessage { get; set; }
        public int SentById { get; set; }
        public int SentToId { get; set; }
    }
}