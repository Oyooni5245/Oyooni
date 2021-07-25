namespace Oyooni.Server.Services.General
{
    public interface INetworkService
    {
        bool IsPortInUse(int port);
        string GetLocalIp();
    }
}
