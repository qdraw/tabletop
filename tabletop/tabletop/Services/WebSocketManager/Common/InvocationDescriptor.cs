using Newtonsoft.Json;

namespace tabletop.Services.WebSocketManager.Common
{
    public class InvocationDescriptor
    {
        [JsonProperty("methodName")]
        public string MethodName { get; set; }

        [JsonPropertyAttribute("arguments")]
        public object[] Arguments { get; set; }
    }
}