using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace tabletop.Services
{
    public class ChatHub : Hub
    {
        public Task Send(string message)
        {
            return Clients.All.InvokeAsync("Send", message);
        }
    }
}
