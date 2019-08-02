using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Cinder.Api.Infrastructure.Dtos
{
    public class ErrorHttpResponseDto
    {
        private static readonly JsonSerializerSettings SerializerSettings =
            new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()};

        public string Message { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ExceptionDto Exception { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, SerializerSettings);
        }
    }
}
