using System.Text.Json;
using Core.Entities;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs
{
    public class MessagingHub : Hub
    {
        private readonly ILogger _logger;

        public MessagingHub(ILogger<MessagingHub> logger)
        {
            _logger = logger;
        }

        public override Task OnConnectedAsync() {
            string ConnectionId = Context.ConnectionId;
            _logger.LogInformation("Connection established");
            _logger.LogCritical("critical log");
            _logger.LogWarning("warning log");
            // _logger.LogInformation("Connection established : " + ConnectionId);
            Clients.Client(ConnectionId).SendAsync("ReceiveConnId", ConnectionId);
            return base.OnConnectedAsync();
        }

        public async Task SendMessageAsync(String message) {
            var routeObj = JsonSerializer.Deserialize<WebsocketMessage>(message);
            _logger.LogInformation("test logging");
            _logger.LogInformation("Message received on: " + Context.ConnectionId);
            _logger.LogInformation("To: " + routeObj.To + ", From: " + routeObj.From + ", Message: " + routeObj.Message);

            // await Clients.Client(routeObj.To.ToString()).SendAsync("ReceiveMessage", message);
            // Console.WriteLine("Message received on: " + Context.ConnectionId);
            await Clients.Client(routeObj.To.ToString()).SendAsync("ReceiveMessage", routeObj);
            
        }
    }
}