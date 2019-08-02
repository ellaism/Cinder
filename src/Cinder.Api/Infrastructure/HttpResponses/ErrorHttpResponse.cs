using System;
using Cinder.Api.Infrastructure.Dtos;

namespace Cinder.Api.Infrastructure.HttpResponses
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

    public static class ErrorHttpResponseExtensions
    {
        public static ErrorHttpResponseDto ToDto(this ErrorHttpResponse source)
        {
            return new ErrorHttpResponseDto {Message = source.Message, Exception = source.Exception?.ToDto()};
        }

        private static ExceptionDto ToDto(this Exception source, int depth = 0)
        {
            depth++;

            if (depth > 10)
            {
                return null;
            }

            ExceptionDto exception = new ExceptionDto
            {
                Message = source.Message,
                HelpLink = source.HelpLink,
                HResult = source.HResult,
                InnerException = source.InnerException?.ToDto(depth),
                Source = source.Source,
                StackTrace = source.StackTrace
            };

            return exception;
        }
    }
}
