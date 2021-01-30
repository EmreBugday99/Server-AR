using NetSync;
using NetSync.Server;
using System;
using System.Numerics;

namespace ServerAR.GameNetwork
{
    public class Player
    {
        private GameLogic _game;
        private GameServer _gameServer;
        private NetworkServer _netServer;

        public Connection PlayerConnection;
        public readonly ushort Id;
        public byte Health;
        public Vector3 Position;

        public DateTime LastAttackTime;

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

            // To prevent uninitialized data.
            LastAttackTime = DateTime.Now;
        }

        public void SyncPlayer()
        {
            // Game state synchronization for the newly joined player.
            for (ushort i = 0; i < _gameServer.Players.Count; i++)
            {
                Player player = _gameServer.Players[i];
                if (Id == player.Id) continue;

                Packet syncPacket = new Packet();
                syncPacket.WriteUnsignedShort(player.Id);
                syncPacket.WriteByte(player.Health);

                _netServer.NetworkSend(PlayerConnection, (byte)GamePackets.SyncPlayer, syncPacket);
            }
        }

        public void SpawnPlayer()
        {
            Packet spawnPacket = new Packet();
            spawnPacket.WriteUnsignedShort(PlayerConnection.ConnectionId);
            spawnPacket.WriteByte(Health);

            _netServer.NetworkSendEveryone((byte)GamePackets.SpawnPlayer, spawnPacket);
        }

        public void SendPositionUpdate(Packet packet, ushort id)
        {
            ushort gridId = packet.ReadUnsignedShort();

            Packet positionPacket = new Packet();
            positionPacket.WriteUnsignedShort(id);
            positionPacket.WriteUnsignedInteger(gridId);

            _netServer.NetworkSendEveryone((byte)GamePackets.ChangePosition, positionPacket);
        }

        public void TakeDamage(byte damageAmount, Player attacker, Player attacked)
        {
            Health -= damageAmount;

            Packet healthPacket = new Packet();
            healthPacket.WriteUnsignedShort(Id);
            healthPacket.WriteByte(Health);

            _netServer.NetworkSendEveryone((byte)GamePackets.UpdateHealth, healthPacket);

            if (Health <= 0)
            {
                Console.WriteLine("Mother fucker is dead");
            }
        }
    }
}