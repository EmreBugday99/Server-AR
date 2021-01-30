using NetSync;
using NetSync.Server;

namespace ServerAR.GameNetwork
{
    public class GameLogic
    {
        private NetworkServer _netServer;
        private GameServer _gameServer;

        private static float _attackDistance = 0.02f;

        public GameLogic(NetworkServer networkServer, GameServer gameServer)
        {
            _netServer = networkServer;
            _gameServer = gameServer;
        }

        public void InitializeGame()
        {
            _netServer.RegisterHandler((byte)GamePackets.ChangePosition, ChangePositionHandler);
            _netServer.RegisterHandler((byte)GamePackets.Attack, AttackHandler);
            _netServer.RegisterHandler((byte)GamePackets.LockGrid, LockGridHandler);
        }

        private void AttackHandler(Connection connection, Packet packet)
        {
            Player attacker = _gameServer.Players[connection.ConnectionId];
            Player attacked = _gameServer.Players[packet.ReadUnsignedShort()];

            if (attacker.Health <= 0) return;

            attacked.TakeDamage(25, attacker, attacked);
        }

        private void ChangePositionHandler(Connection connection, Packet packet)
        {
            _gameServer.Players[connection.ConnectionId].SendPositionUpdate(packet, connection.ConnectionId);
        }

        private void LockGridHandler(Connection connection, Packet packet)
        {
            ushort gridId = packet.ReadUnsignedShort();

            Packet lockPacket = new Packet();
            lockPacket.WriteUnsignedShort(gridId);

            _netServer.NetworkSendEveryone((byte)GamePackets.LockGrid, lockPacket);
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