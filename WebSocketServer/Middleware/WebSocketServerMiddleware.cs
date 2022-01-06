using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebSocketServer.GameManager;
using WebSocketServer.Messengers;

namespace WebSocketServer.Middleware
{
    public class WebSocketServerMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly WebSocketServerConnectionManager _manager;

        public WebSocketServerMiddleware(RequestDelegate next,WebSocketServerConnectionManager manager)
            
        {
            _next = next;
            _manager = manager;
        }

        public async Task InvokeAsync(HttpContext context, ConnectionMessenger _connectionMessenger)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                WebSocket websocket = await context.WebSockets.AcceptWebSocketAsync();
                _connectionMessenger.Log();
                string connId = _manager.AddSocket(websocket);

                await _connectionMessenger.SendMessageAsync(websocket);
                //await SendGameState(websocket,connId);

                await ReceiveMessage(websocket, async (result, buffer) =>
                {
                    if (result.MessageType == WebSocketMessageType.Text)
                    { 
                        Console.WriteLine("Message Recieved");
                        Console.WriteLine(Encoding.UTF8.GetString(buffer,0,result.Count));
                        var msg = await RouteJSONMessageAsync(Encoding.UTF8.GetString(buffer, 0, result.Count));
                        await SendConnectedAsync(websocket, msg);
                        return;
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        Console.WriteLine("Recieved Close message");
                        _manager.GetAllSockets().TryRemove(connId, out WebSocket sock);
                        
                        await sock.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);

                        return;
                    }
                });
            }
            else
            {
                await _next(context);
            }
        }

        private async Task SendConnIdAsync(WebSocket socket, string connId)
        {
            var buffer = Encoding.UTF8.GetBytes(connId);
            await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private async Task SendConnectedAsync(WebSocket socket,string msg)
        {
            var buffer = Encoding.UTF8.GetBytes(msg);
            await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private async Task SendNewGameAsync(WebSocket socket)
        {
            var buffer = Encoding.UTF8.GetBytes(Message.NewGame);
            await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private async Task SendGameState(WebSocket socket, string connId)
        {
            Game game = new Game();
            game.GameId = Guid.NewGuid();
            game.GameState = new int[3,3] { {0,0,0},{0,0,0},{0,0,0} };
            game.Players.Add(new Player()
            {
                YouStart = true,
                ConnId = connId,
                YourMove = true
            });
            game.IsFinished = false;

            string message = JsonConvert.SerializeObject(game);

            var buffer = Encoding.UTF8.GetBytes(message);

            await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);

        }

        private async Task ReceiveMessage(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
        {
            var buffer = new Byte[1024 * 4];

            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer),
                    cancellationToken: CancellationToken.None);

                handleMessage(result, buffer);
            }

        }

        public async Task<string> RouteJSONMessageAsync(string message)
        {
            var route = JsonConvert.DeserializeObject<dynamic>(message);

            if(route.Message == "NewGame")
            {
                return Message.NewGame;
            }

            var x = route.GamePos;

            Console.WriteLine("");

            for (var i=0;i<3;i++)
            {
                for(var j=0;j<3;j++)
                {
                    Console.Write(x[i][j]);
                }
                Console.WriteLine("");
            }

            return "";
        }

    }
}
