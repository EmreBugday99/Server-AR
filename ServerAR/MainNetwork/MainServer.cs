using NetSync;
using NetSync.Server;
using NetSync.Transport.AsyncTcp;
using System;

namespace ServerAR.MainNetwork
{
    internal class MainServer
    {
        public static NetworkServer NetServer;

        internal MainServer(int port, ushort maxConnections)
        {
            if (NetServer != null) return;

            AsyncTcp transport = new AsyncTcp();
            NetServer = new NetworkServer(port, maxConnections, 4095, transport);
            InitializeServer();
        }

        private void InitializeServer()
        {
            NetServer.OnServerStarted += OnServerStart;
            NetServer.RegisterHandler((byte)MainPackets.CreateMatch, CreateMatchHandler);
        }

        private void CreateMatchHandler(Connection connection, Packet packet)
        {
            bool isPublic = packet.ReadBool();
            MatchMaker.CreateGameServer(connection, isPublic);
        }

        public void Start()
        {
            NetServer.StartServer();
        }

        private void OnServerStart(NetworkServer server)
        {
            Console.WriteLine("NetSync started listening for connections.");
        }
    }
}