namespace API.Dtos
{
    public class MessageToReturnDto
    {
        public int Id { get; set; }
        public string TextMessage { get; set; }
        public DateTime TimeOfMessage { get; set; }
        public int SentByUserId { get; set; }
        public int SentToUserId { get; set; }
    }
}