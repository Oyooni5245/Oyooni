using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Oyooni.Server.Hubs
{
    /// <summary>
    /// Represents the application's main hub
    /// </summary>
    public class ApplicationHub : Hub
    {
        /// <summary>
        /// Handles when a client connects to the hub
        /// </summary>
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        /// <summary>
        /// Handles when a client is disconnected from the hub
        /// </summary>
        /// <param name="exception">The exception thrown upon disconnection</param>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
