using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Negrisan_Mihai_Lab2.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", Context.User.Identity.Name, message, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
