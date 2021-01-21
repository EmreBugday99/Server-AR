namespace ServerAR.GameNetwork
{
    public enum GamePackets : byte
    {
        SpawnPlayer = 0,
        SyncPlayer,
        ChangePosition,
        Attack,
        UpdateHealth
    }
}