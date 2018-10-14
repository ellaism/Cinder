using System;

namespace EllaX.Logic.Clients.Responses
{
    public class Response<TResult> where TResult : IResult
    {
        public string JsonRpc { get; set; }
        public TResult Result { get; set; }
        public Guid Id { get; set; }
    }
}
