using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using tabletop.Services.WebSocketManager;
using tabletop.Services.WebSocketManager.Common;

namespace tabletop.Handlers
{
    public class NotificationsHandler : WebSocketHandler
    {
        public NotificationsHandler(WebSocketConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager)
        {

        }

        /// <inheritdoc />
        public override async Task OnConnected(WebSocket socket)
        {
            WebSocketConnectionManager.AddSocket(socket);

            WebSocketConnectionManager.AddToGroup(WebSocketConnectionManager.GetId(socket),"test");

            await SendMessageAsync(socket, new Message()
            {
                MessageType = MessageType.ConnectionEvent,
                Data = WebSocketConnectionManager.GetId(socket)
            }).ConfigureAwait(false);

        }

        //public async Task SendMessage(string socketId, string message)
        //{
        //    await InvokeClientMethodToAllAsync("receiveMessage", socketId, message);
        //}



        //public async Task SendMessage(string socketId, string message)
        //{
        //    await InvokeClientMethodToAllAsync("receiveMessage", socketId, message);
        //}

        public async Task IsInUse(string groupId)
        {
            await InvokeClientMethodToGroupAsync(groupId, "isInUse");
            await InvokeClientMethodToAllAsync("isInUse-public");
        }

    }
}
