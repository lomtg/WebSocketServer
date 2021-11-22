using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Collections.Concurrent;

namespace WebSocketServer.Middleware
{
    public class WebSocketServerConnectionManager
    {
        private readonly ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();

        public ConcurrentDictionary<string, WebSocket> GetAllSockets()
        {
            return _sockets;
        }

        public string AddSocket(WebSocket socket)
        {
            string connID = Guid.NewGuid().ToString();

            _sockets.TryAdd(connID, socket);
            Console.WriteLine("Connection Added " + connID);

            return connID;
        }
    }
}
