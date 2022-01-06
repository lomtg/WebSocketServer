using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocketServer.Messengers;

namespace WebSocketServer.Messengers
{
    public class ConnectionMessenger : Messenger
    {
        public override string Msg { get => Message.Connected;}

        public async override Task SendMessageAsync(WebSocket socket)
        {
            var buffer = Encoding.UTF8.GetBytes(Msg);
            await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
