using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using EllaX.Api.Infrastructure.Dtos;
using EllaX.Api.Infrastructure.HttpResponses;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EllaX.Api.Infrastructure.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly IHostingEnvironment _env;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly IMapper _mapper;
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next, IHostingEnvironment env, IMapper mapper,
            ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _env = env;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(context, e);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string error = "An unexpected error occured.";
            string logError = "Unexpected exception caught";
            switch (exception)
            {
                //case ResourceException _:
                //    statusCode = HttpStatusCode.Forbidden;
                //    error = "There was an issue accessing the requested object.";
                //    logError = "ResourceException exception caught";
                //    break;
                case DbUpdateConcurrencyException _:
                    statusCode = HttpStatusCode.Conflict;
                    error = "There was an issue saving changes to the object.";
                    logError = "DbUpdateConcurrency exception caught";
                    break;
                case DbUpdateException _:
                    statusCode = HttpStatusCode.BadRequest;
                    error = "There was an issue saving changes to the object.";
                    logError = "DbUpdate exception caught";
                    break;
                case InvalidOperationException _:
                    statusCode = HttpStatusCode.BadRequest;
                    logError = "Invalid operation exception caught";
                    break;
            }

            _logger.LogError(exception, logError);

            ErrorHttpResponse errorResponse = new ErrorHttpResponse(error);
            if (_env.IsDevelopment())
            {
                errorResponse.Exception = exception;
            }

            ErrorHttpResponseDto body = _mapper.Map<ErrorHttpResponseDto>(errorResponse);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) statusCode;

            return context.Response.WriteAsync(body.ToString());
        }
    }
}
