using System.Text.Json;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc;


namespace API.Hubs
{
    public class MessagingHub : Hub
    {
        private readonly ILogger _logger;
        private readonly IGenericRepository<Message> _repo;
        private static Dictionary<int, String> _userIdToConnectionIdMap = new Dictionary<int, String>();
        private static Dictionary<String, int> _connectionIdToUserIdMap = new Dictionary<String, int>();


        public MessagingHub(ILogger<MessagingHub> logger, IGenericRepository<Message> repo)
        {
            this._logger = logger;
            this._repo = repo;
        }

        public override Task OnConnectedAsync() {
            string ConnectionId = Context.ConnectionId;
            Clients.Client(ConnectionId).SendAsync("receiveConnId", ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception e) {
            string ConnectionId = Context.ConnectionId;
            _logger.LogInformation("removing from userIdToConnectionIdMap: " + JsonSerializer.Serialize(_userIdToConnectionIdMap));
            _logger.LogInformation("removing from connectionIdToUserIdMap: " + JsonSerializer.Serialize(_connectionIdToUserIdMap));

            _userIdToConnectionIdMap.Remove(_connectionIdToUserIdMap[ConnectionId]);
            _logger.LogInformation("removing from connectionIdToUserIdMap: " + JsonSerializer.Serialize(_connectionIdToUserIdMap));
            _connectionIdToUserIdMap.Remove(ConnectionId);

            _logger.LogInformation("ConnectionId: " + ConnectionId + " has lost connection and has been removed from map");
            _logger.LogInformation(JsonSerializer.Serialize(_userIdToConnectionIdMap));
            _logger.LogInformation(JsonSerializer.Serialize(_connectionIdToUserIdMap));

            return base.OnDisconnectedAsync(e);
        }

        public async Task AddUserToMap(String userIdCombo) {
            String ConnectionId = Context.ConnectionId;
            _logger.LogInformation("USERIDCOMBO: " + userIdCombo);
            var routeObj = JsonSerializer.Deserialize<UserIdCombo>(userIdCombo);
            _connectionIdToUserIdMap.Add(ConnectionId, routeObj.UserId);
            _userIdToConnectionIdMap.Add(routeObj.UserId, ConnectionId);
            _logger.LogInformation("userIdToConnectionIdMap: " + JsonSerializer.Serialize(_userIdToConnectionIdMap));
            _logger.LogInformation("connectionIdToUserIdMap: " + JsonSerializer.Serialize(_connectionIdToUserIdMap));
            await Clients.Client(routeObj.ConnectionId.ToString()).SendAsync("userAddedToMap", _connectionIdToUserIdMap);
        }

        public async Task<ActionResult<int>> AddMessageToDB(WebsocketMessage message) {
            var results = await _repo.CreateEntity(new Message {
                TextMessage = message.Message,
                TimeOfMessage = DateTime.Now,
                SentByUserId = message.From,
                SentToUserId = message.To
            });

            return results;
        }

        public async Task SendMessageAsync(String message) {
            var routeObj = JsonSerializer.Deserialize<WebsocketMessage>(message);
            _logger.LogInformation("Message received on: " + Context.ConnectionId);

            ActionResult<int> dbMessage = await AddMessageToDB(routeObj);
            _logger.LogInformation("Message saved to database: " + dbMessage);

            if (_userIdToConnectionIdMap.ContainsKey(routeObj.To)) {

                await Clients.Client(_userIdToConnectionIdMap[routeObj.To]).SendAsync("receiveMessage", routeObj);
            }
        }
    }
}