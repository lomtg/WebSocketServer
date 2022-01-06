using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace WebSocketServer.Messengers
{
    public abstract class Messenger
    {
        public abstract string Msg { get;}
        public void Log()
        {
            Console.WriteLine(Msg);
        }

        public abstract Task SendMessageAsync(WebSocket socket);
    }
}
