using NetSync;
using NetSync.Client;
using NetSync.Transport.AsyncTcp;
using ServerAR.MainNetwork;
using System;
using System.Threading;

internal class Client
{
    private NetworkClient client;

    internal void Start()
    {
        AsyncTcp transport = new AsyncTcp();
        client = new NetworkClient("206.81.21.21", 2405, 4095, transport);

        Initialize();
        client.StartClient();
    }

    private void Initialize()
    {
        client.OnClientConnected += OnConnected;
        client.OnClientDisconnected += () => Console.WriteLine("AAA");
        client.OnClientErrorDetected += Console.WriteLine;
        client.RegisterHandler((byte)MainPackets.JoinMatch, JoinMatchHandler);
    }

    private void OnConnected()
    {
        Thread.Sleep(100);

        Packet createMatchPacket = new Packet();
        createMatchPacket.WriteBool(true);
        client.NetworkSend((byte)MainPackets.CreateMatch, createMatchPacket);

        //Packet joinMatchPacket = new Packet();
        //joinMatchPacket.WriteInteger(36673);
        //client.NetworkSend((byte)MainPackets.JoinMatch, joinMatchPacket);
    }

    private void JoinMatchHandler(Packet packet)
    {
        Console.WriteLine("Join Match Received");
        int port = packet.ReadInteger();
        Console.WriteLine(port);
        AsyncTcp matchTransport = new AsyncTcp();
        NetworkClient matchClient = new NetworkClient("206.81.21.21", port, 4095, matchTransport);

        matchClient.StartClient();
        //matchClient.StopClient();
    }
}