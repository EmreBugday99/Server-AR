using NetSync.Server;
using ServerAR.GameNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using NetSync.Transport.AsyncTcp;

namespace ServerAR.MainNetwork
{
    public static class MatchMaker
    {
        public static Dictionary<int, GameServer> GameServers = new Dictionary<int, GameServer>();

        public static void CreateGameServer(Connection connection, bool isPublic)
        {
            AsyncTcp newTransport = new AsyncTcp();
            NetworkServer newServer = new NetworkServer(0, 2, 4095, newTransport);

            GameServer gameServer = new GameServer(newServer, isPublic, connection);
        }
    }
}