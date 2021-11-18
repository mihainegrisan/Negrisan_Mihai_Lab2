using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Negrisan_Mihai_Lab2.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
