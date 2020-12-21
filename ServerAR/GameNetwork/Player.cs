using NetSync.Server;

namespace ServerAR.GameNetwork
{
    public class Player
    {
        public Connection PlayerConnection;

        public Player(Connection playerConnection)
        {
            PlayerConnection = playerConnection;
        }
    }
}