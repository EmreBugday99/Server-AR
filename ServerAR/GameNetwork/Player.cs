using NetSync;
using NetSync.Server;
using System.Numerics;

namespace ServerAR.GameNetwork
{
    public class Player
    {
        private GameLogic _game;
        private GameServer _gameServer;
        private NetworkServer _netServer;

        public Connection PlayerConnection;
        public ushort Id;
        public byte Health;
        public Vector3 Position;

        public Player(Connection playerConnection, GameLogic game, GameServer gameServer, NetworkServer netServer)
        {
            PlayerConnection = playerConnection;
            _game = game;
            _gameServer = gameServer;
            _netServer = netServer;
            Health = 100;
            Id = playerConnection.ConnectionId;

            if (Id == 0)
            {
                Position = new Vector3(2.0f, 0.0f, 0.0f);
            }
            else
            {
                Position = new Vector3(-2.0f, 0.0f, 0.0f);
            }
        }

        public void SyncPlayer()
        {
            for (ushort i = 0; i < _gameServer.Players.Count; i++)
            {
                Player player = _gameServer.Players[i];

                Packet syncPacket = new Packet();
                syncPacket.WriteByte(Health);
                syncPacket.WriteUnsignedShort(Id);
                syncPacket.WriteFloat(Position.X);
                syncPacket.WriteFloat(Position.Y);
                syncPacket.WriteFloat(Position.Z);

                _netServer.NetworkSend(PlayerConnection, (byte)GamePackets.SyncPlayer, syncPacket);
            }
        }

        public void SpawnPlayer()
        {
            Packet spawnPacket = new Packet();
            spawnPacket.WriteByte(Health);
            spawnPacket.WriteUnsignedShort(Id);
            spawnPacket.WriteFloat(Position.X);
            spawnPacket.WriteFloat(Position.Y);
            spawnPacket.WriteFloat(Position.Z);

            _netServer.NetworkSendEveryone((byte)GamePackets.SpawnPlayer, spawnPacket);
        }
    }
}