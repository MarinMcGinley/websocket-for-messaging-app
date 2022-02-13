using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Core.WebsocketEntities;

namespace API.Websocket
{
    public class WebSocketHelper
    {

        public static async Task Echo(HttpContext context, WebSocket webSocket, ILogger _logger)
        {
            _logger.LogInformation("Starting websocket connection: " + DateTime.Now);
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {

                await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);

                _logger.LogInformation("MESSAGE: " + Encoding.UTF8.GetString(buffer, 0, result.Count));
                // try {
                //     _logger.LogInformation("body of response: " + context.Response.ToString());
                //     WebsocketMessage websocketMessage = JsonSerializer.Deserialize<WebsocketMessage>(buffer);
                //     _logger.LogInformation(JsonSerializer.Serialize(websocketMessage));
                // } catch (Exception e) {
                //     _logger.LogError(e.Message, e.StackTrace);
                // }
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                _logger.LogInformation("-----------result");
                _logger.LogInformation(result.ToString());
                _logger.LogInformation("-----------result");
                
                _logger.LogInformation("Time: " + DateTime.Now);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            _logger.LogInformation("Closing websocket connection: " + DateTime.Now);

        }
    }
}