using System;

namespace EllaX.Api.Infrastructure.HttpResponses
{
    public class ErrorHttpResponse
    {
        public ErrorHttpResponse(string message)
        {
            Message = message;
        }

        public string Message { get; }
        public Exception Exception { get; set; }
    }
}
