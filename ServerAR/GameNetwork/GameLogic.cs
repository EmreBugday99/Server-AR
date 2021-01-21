using System;
using NetSync;
using NetSync.Server;
using System.Numerics;

namespace ServerAR.GameNetwork
{
    public class GameLogic
    {
        private NetworkServer _netServer;
        private GameServer _gameServer;

        private static float _attackDistance = 2.0f;

        public GameLogic(NetworkServer networkServer, GameServer gameServer)
        {
            _netServer = networkServer;
            _gameServer = gameServer;
        }

        public void InitializeGame()
        {
            _netServer.RegisterHandler((byte)GamePackets.ChangePosition, ChangePositionHandler);
            _netServer.RegisterHandler((byte)GamePackets.Attack, AttackHandler);
        }

        private void AttackHandler(Connection connection, Packet packet)
        {
            Player attacker = _gameServer.Players[connection.ConnectionId];
            Player attacked = _gameServer.Players[packet.ReadUnsignedShort()];

            if (attacker.Health <= 0) return;
            if (Vector3.Distance(attacker.Position, attacked.Position) >= _attackDistance) return;
            //if((DateTime.Now - attacker.LastAttackTime).Seconds < 1.0f) return;

            attacked.TakeDamage(25);
        }

        private void ChangePositionHandler(Connection connection, Packet packet)
        {
            _gameServer.Players[connection.ConnectionId].SendPositionUpdate(packet);
        }

        public void OnPlayerConnect(Player player)
        {
            //Syncing player with the current game state.
            player.SyncPlayer();
            //Notifying other players about the newly joined player.
            player.SpawnPlayer();
        }
    }
}