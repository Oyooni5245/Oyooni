using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Oyooni.Server.Hubs
{
    /// <summary>
    /// Represents the application's main hub
    /// </summary>
    [Authorize]
    public class ApplicationHub : Hub<IServerMethods>
    {
        /// <summary>
        /// Handles when a client connects to the hub
        /// </summary>
        public override async Task OnConnectedAsync()
        {
            // Send to the client that he connected successfully
            await Clients.Caller.ConnectedSuccessfully(Context.ConnectionId);

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

    public interface IServerMethods
    {
        Task ConnectedSuccessfully(string connectionId);
    }
}
