using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSocketServer.GameManager
{
    public class Player
    {
        public string ConnId { get; set; }

        public bool YourMove { get; set; }
        
        public bool YouStart { get; set; }
    }
}
