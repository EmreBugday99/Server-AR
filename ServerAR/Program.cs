using ServerAR.MainNetwork;
using System;

namespace ServerAR
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("AR-Server is starting...");
            MainServer mainServer = new MainServer(2405, 10);
            mainServer.Start();

            while (true)
            {
            }
        }
    }
}