using Microsoft.Extensions.DependencyInjection;
using Oyooni.Server.Attributes;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Oyooni.Server.Services.General
{
    [Injected(ServiceLifetime.Singleton, typeof(INetworkService), ignoreForNow: false)]
    public class NetworkService : INetworkService
    {
        public string GetLocalIp()
        {
            var localIp = string.Empty;

            foreach (var ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIp = ip.ToString();
                }
            }

            return localIp;
        }

        public bool IsPortInUse(int port)
            => IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners().Any(p => p.Port == port);
    }
}
