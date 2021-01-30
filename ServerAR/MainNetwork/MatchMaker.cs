using NetSync;
using NetSync.Server;
using ServerAR.GameNetwork;
using System;
using System.Collections.Generic;

namespace ServerAR.MainNetwork
{
    public static class MatchMaker
    {
        public static Dictionary<int, GameServer> GameServers = new Dictionary<int, GameServer>();

        public static void CreateGameServer(Connection connection, bool isPublic)
        {
            GameServer gameServer = new GameServer(isPublic, connection);
        }

        public static void JoinGameServer(Connection connection, int matchId)
        {
            if (GameServers.ContainsKey(matchId) == false)
            {
                Console.WriteLine($"Wrong game server {matchId}");
                return;
            }

            Packet joinPacket = new Packet();
            joinPacket.WriteInteger(matchId);

            MainServer.NetServer.NetworkSend(connection, (byte)MainPackets.JoinMatch, joinPacket);
        }
    }
}