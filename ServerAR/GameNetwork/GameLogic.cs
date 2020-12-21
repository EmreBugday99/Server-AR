using System;
using NetSync.Server;
using System.Threading;
using NetSync;

namespace ServerAR.GameNetwork
{
    public class GameLogic
    {
        private NetworkServer _netServer;
        private GameServer _gameServer;

        public GameLogic(NetworkServer networkServer, GameServer gameServer)
        {
            _netServer = networkServer;
            _gameServer = gameServer;
        }

        public void InitializeGame()
        {
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