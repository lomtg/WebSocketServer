using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSocketServer.GameManager
{
    public class Game
    {
        public Guid GameId { get; set; }

        public List<Player> Players = new List<Player>();

        public int[,] GameState { get; set; }

        public bool IsFinished { get; set; }

    }
}
