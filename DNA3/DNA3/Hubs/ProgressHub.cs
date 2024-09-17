#region Usings

using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

#endregion

namespace DNA3.Hubs {

    public class ProgressHub : Hub {

        public async Task SendUpdate(string value) {
            await Clients.All.SendAsync("UpdateProgress", value);
        }

    }

}