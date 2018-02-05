using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace tabletop.Services.WebSocketManager
{
    public class WebSocketConnectionManager
    {
        private readonly ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();
        private readonly ConcurrentDictionary<string, List<string>> _groups = new ConcurrentDictionary<string, List<string>>();

        public WebSocket GetSocketById(string id)
        {
            return _sockets.FirstOrDefault(p => p.Key == id).Value;
        }

        public ConcurrentDictionary<string, WebSocket> GetAll()
        {
            return _sockets;
        }

        public List<string> GetAllFromGroup(string groupId)
        {
            if (_groups.ContainsKey(groupId))
            {
                return _groups[groupId];
            }

            return default(List<string>);
        }

        public string GetId(WebSocket socket)
        {
            return _sockets.FirstOrDefault(p => p.Value == socket).Key;
        }

        public void AddSocket(WebSocket socket)
        {
            _sockets.TryAdd(CreateConnectionId(), socket);
        }

        public void AddToGroup(string socketId, string groupId)
        {
            if (_groups.ContainsKey(groupId))
            {
                var list = _groups[groupId];
                list.Add(socketId);
                _groups[groupId] = list;

                return;
            }

            _groups.TryAdd(groupId, new List<string> { socketId });
        }

        public void RemoveFromGroup(string socketId, string groupId)
        {
            if (_groups.ContainsKey(groupId))
            {
                var list = _groups[groupId];
                list.Remove(socketId);
                _groups[groupId] = list;

                return;
            }
        }

        public async Task RemoveSocket(string id)
        {
            WebSocket socket;
            _sockets.TryRemove(id, out socket);

            await socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                                    statusDescription: "Closed by the WebSocketManager",
                                    cancellationToken: CancellationToken.None).ConfigureAwait(false);
        }

        private string CreateConnectionId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
