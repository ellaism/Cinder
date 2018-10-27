using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace EllaX.Clients.Requests
{
    public class Message
    {
        protected Message() { }

        [JsonProperty(PropertyName = "jsonrpc")]
        public string Version { get; set; } = "2.0";

        [JsonProperty(PropertyName = "method")]
        public string Method { get; set; }

        [JsonProperty(PropertyName = "params")]
        public IList<object> Parameters { get; set; }

        [JsonProperty(PropertyName = "id")] public Guid Id { get; set; } = Guid.NewGuid();

        public static Message CreateMessage(string method)
        {
            return new Message {Method = method};
        }

        public static Message CreateMessage(string method, IList<object> parameters)
        {
            return new Message {Method = method, Parameters = parameters};
        }
    }
}
