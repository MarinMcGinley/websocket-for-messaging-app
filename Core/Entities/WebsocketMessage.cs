namespace Core.Entities
{
    public class WebsocketMessage
    {
        public int From { get; set; }
        public int To { get; set; }
        public string Message { get; set; }
    }
}