using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using tabletop.Services.WebSocketManager;

namespace tabletop.MessageHandler
{
    public class NotificationsMessageHandler : WebSocketHandler
    {
        public NotificationsMessageHandler(WebSocketConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager)
        {
        }

        public async Task SendMessage(string socketId, string message)
        {
            await InvokeClientMethodToAllAsync("receiveMessage", socketId, message);
        }

        public async void IsFree(string groupId)
        {

            await InvokeClientMethodToGroupAsync(groupId,"isFree", false);

            await InvokeClientMethodToAllAsync(groupId +"__", false);
        }

    }
}
