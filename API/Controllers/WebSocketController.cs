using System.Net;
using System.Net.WebSockets;
using API.Websocket;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class WebSocketController : BaseApiController
    {
        private readonly ILogger<WebSocketController> _logger;
        public WebSocketController(ILogger<WebSocketController> logger)
        {
            _logger = logger;
        }

        [HttpGet("test")]
        public async Task<ActionResult> GetTest() {
            _logger.LogInformation("Called api/websocket/test");

            return Ok();
        }

        [HttpGet("message")]
        public async Task CreateWebsocketConnection()
        {
            _logger.LogInformation(DateTime.Now + ": Called api/websocket/message");
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                _logger.LogInformation("IS WEBSOCKET REQUEST");
                using WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await WebSocketHelper.Echo(HttpContext, webSocket, _logger);
                _logger.LogInformation("WEBSOCKET");
            }
            else
            {
                HttpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
            }
            // return Ok();
        }
    }
}