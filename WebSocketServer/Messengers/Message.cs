using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSocketServer.Messengers
{
    public static class Message
    {
        public static string Connected => "Connected";
        public static string NewGame => "Searching";
        public static string OponnetFound => "Oponnent Found";
        public static string OponnetMove => "Oponnent Move";
        public static string YourMove => "Your Move";
        public static string GameEnded => "Game Ended";
    }
}
