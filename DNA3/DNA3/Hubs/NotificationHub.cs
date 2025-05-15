#region Usings

using DNA3.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#endregion

namespace DNA3.Hubs {

    public class NotificationHub(ILogger<NotificationHub> logger) : Hub {

        #region Variables

        private readonly ILogger<NotificationHub> Logger = logger;

        #endregion

        #region Class Methods

        // On Connect (Async)
        public override System.Threading.Tasks.Task OnConnectedAsync() {
            Groups.AddToGroupAsync(Context.ConnectionId, GetGroupName());
            return base.OnConnectedAsync();
        }

        // On Disconnect (Async)
        public override System.Threading.Tasks.Task OnDisconnectedAsync(Exception e) {
            Groups.RemoveFromGroupAsync(Context.ConnectionId, GetGroupName());
            return base.OnDisconnectedAsync(e);
        }

        // Send
        public async void Send(string type, string content, string title) {
            try {
                await Clients.OthersInGroup(GetGroupName()).SendAsync("displayMessage", type, content, title);
            } catch (Exception ex) {
                Logger.LogError(ex, "{message}", ex.Message);
            }
        }

        // Get Group Name
        public string GetGroupName() {
            return GetRemoteIpAddress();
        }

        // Get Remote IP Address
        public string GetRemoteIpAddress() {
            return Context.GetHttpContext()?.Connection.RemoteIpAddress.ToString();
        }

        #endregion

    }

}
