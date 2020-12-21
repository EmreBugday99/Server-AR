using NetSync;
using NetSync.Server;
using ServerAR.MainNetwork;
using System;
using System.Collections.Generic;

namespace ServerAR.GameNetwork
{
    public class GameServer
    {
        public NetworkServer NetServer;
        public Connection HostConnection;

        public readonly bool IsPublic;
        public int MatchId;

        public Dictionary<ushort, Player> Players = new Dictionary<ushort, Player>();

        public GameServer(NetworkServer networkServer, bool isPublic, Connection hostConnection)
        {
            NetServer = networkServer;
            IsPublic = isPublic;
            HostConnection = hostConnection;

            InitializeServer();
            NetServer.StartServer();
        }

        private void InitializeServer()
        {
            NetServer.OnServerStarted += OnServerStart;
            NetServer.OnServerStopped += OnServerStop;
            NetServer.OnServerConnected += OnConnect;
            NetServer.OnServerDisconnected += OnDisconnect;
        }

        private void OnServerStart(NetworkServer server)
        {
            MatchMaker.GameServers.Add(NetServer.ServerPort, this);
            MatchId = NetServer.ServerPort;

            Console.WriteLine($"A new server started on Port: {MatchId}");

            //Client may have disconnected from the main server for some reason. This is for safety.
            if (MainServer.NetServer.Connections[HostConnection.ConnectionId].IsConnected == false)
            {
                Console.WriteLine("fuck up");
                return;
            }

            Packet joinMatchPacket = new Packet();
            joinMatchPacket.WriteInteger(MatchId);
            MainServer.NetServer.NetworkSend(HostConnection, (byte)MainPackets.JoinMatch, joinMatchPacket);
        }

        private void OnServerStop(NetworkServer server)
        {
            MatchMaker.GameServers.Remove(NetServer.ServerPort);
            Console.WriteLine($"GameServer[{NetServer.ServerPort}] stopped running");
        }

        private void OnConnect(Connection connection)
        {
            Console.WriteLine($"A new player has connected to GameServer[{MatchId}]");

            Player newPlayer = new Player(connection);
            Players.Add(connection.ConnectionId, newPlayer);
        }

        private void OnDisconnect(Connection connection)
        {
            Console.WriteLine($"A player has disconnected from GameServer[{MatchId}]");

            Players.Remove(connection.ConnectionId);

            if (Players.Count == 0)
                NetServer.StopServer();
        }
    }
}