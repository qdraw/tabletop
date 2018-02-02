using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using tabletop.MessageHandler;


namespace tabletop.Controllers
{
    public class MessagesController : Controller
    {
        private NotificationsMessageHandler NotificationsMessageHandler { get; }

        public MessagesController(NotificationsMessageHandler notificationsMessageHandler)
        {
            NotificationsMessageHandler = notificationsMessageHandler;
        }

        [HttpGet]
        public async Task IsFree([FromQueryAttribute]string message)
        {
            await NotificationsMessageHandler.IsFree(true);
        }
    }

    //public class MessagesController : Controller
    //{
    //    private NotificationsMessageHandler NotificationsMessageHandler { get; }

    //    public MessagesController(NotificationsMessageHandler notificationsMessageHandler)
    //    {
    //        NotificationsMessageHandler = notificationsMessageHandler;
    //    }

    //    [HttpGet]
    //    public async Task SendMessage([FromQueryAttribute]string message)
    //    {

    //        await NotificationsMessageHandler.InvokeClientMethodToAllAsync("receiveMessage", message);
    //    }
    //}
}
