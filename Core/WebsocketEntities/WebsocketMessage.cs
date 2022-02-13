namespace Core.WebsocketEntities
{
    public class WebsocketMessage
    {
        public string message { get; set; }
        public int fromUserId { get; set; }
        public int toUserId { get; set; }
    }
}