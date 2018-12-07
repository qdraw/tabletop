using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using tabletop.Interfaces;

namespace tabletop.Hubs
{
    public class DataHub : Hub
    {
        public Task Broadcast(string sender, string measurement)
        {
            return Clients
                // Do not Broadcast to Caller:
                .AllExcept(new[] { Context.ConnectionId })
                // Broadcast to all connected clients:
                .SendAsync("Broadcast", sender, measurement);
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("Send", $"{Context.ConnectionId} joined");
        }

        public Task Pong(string message)
        {
            return Clients.Client(Context.ConnectionId).SendAsync("Pong", $"{Context.ConnectionId}: {message}");
        }

        //public override async Task OnDisconnectedAsync(Exception ex)
        //{
        //    await Clients.All.InvokeAsync("Send", $"{Context.ConnectionId} left");
        //}

        public Task Send(string message)
        {
            return Clients.All.SendAsync("Send", $"{Context.ConnectionId}: {message}");
        }

        //public Task SendToGroup(string groupName, string message)
        //{
        //    return Clients.Group(groupName).InvokeAsync("Send", $"{Context.ConnectionId}@{groupName}: {message}");
        //}

        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} joined {groupName}");
        }

        //public async Task LeaveGroup(string groupName)
        //{
        //    await Groups.RemoveAsync(Context.ConnectionId, groupName);

        //    await Clients.Group(groupName).InvokeAsync("Send", $"{Context.ConnectionId} left {groupName}");
        //}

        //public Task Echo(string message)
        //{
        //    return Clients.Client(Context.ConnectionId).InvokeAsync("Send", $"{Context.ConnectionId}: {message}");
        //}
    }
}
