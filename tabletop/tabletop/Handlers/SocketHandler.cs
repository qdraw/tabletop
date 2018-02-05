using System;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;


namespace tabletop.Handlers
{

    public class SocketHandler
    {
        public const int BufferSize = 4096;

        private readonly WebSocket _socket;

        public SocketHandler(WebSocket socket)
        {
            _socket = socket;
        }

        public async Task SendHello()
        {

            const string text = "Hello World";
            var token = CancellationToken.None;
            const WebSocketMessageType type = WebSocketMessageType.Text;
            var data = Encoding.UTF8.GetBytes(text);
            var buffer = new ArraySegment<Byte>(data);

            while (_socket.State == WebSocketState.Open)
            {
                await _socket.SendAsync(buffer, type, true, token);
            }
        }


        private async Task EchoLoop()
        {
            var buffer = new byte[BufferSize];
            var seg = new ArraySegment<byte>(buffer);

            while (_socket.State == WebSocketState.Open)
            {
                var incoming = await _socket.ReceiveAsync(seg, CancellationToken.None);
                var outgoing = new ArraySegment<byte>(buffer, 0, incoming.Count);
                await _socket.SendAsync(outgoing, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        private static async Task Acceptor(HttpContext httpContext, Func<Task> next)
        {
            if (!httpContext.WebSockets.IsWebSocketRequest)
                return;

            var socket = await httpContext.WebSockets.AcceptWebSocketAsync();
            var handler = new SocketHandler(socket);
            await handler.SendHello();
        }

        public static void Map(IApplicationBuilder app)
        {
            app.UseWebSockets();
            app.Use(Acceptor);
        }




    }



}
