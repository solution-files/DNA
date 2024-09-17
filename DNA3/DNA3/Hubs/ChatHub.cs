#region Usings

using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

#endregion

namespace DNA3.Hubs {

    public class ChatHub : Hub {

        public async Task SendMessage(string user, string message) {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

    }

}