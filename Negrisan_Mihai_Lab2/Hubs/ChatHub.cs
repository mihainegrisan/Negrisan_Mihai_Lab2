using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Negrisan_Mihai_Lab2.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string firstName, string lastName, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", firstName, lastName, message, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
