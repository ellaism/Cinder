using System;

namespace EllaX.Clients.Responses
{
    public class Response<TResult>
    {
        public string JsonRpc { get; set; }
        public ErrorResponse Error { get; set; }
        public TResult Result { get; set; }
        public Guid Id { get; set; }
        public bool IsError => Error != null;

        public class ErrorResponse
        {
            public int Code { get; set; }
            public string Message { get; set; }
        }
    }
}
