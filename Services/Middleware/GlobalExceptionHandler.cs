using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace northwindAPI.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken token)
        {
            _logger.LogError(exception,exception.Message);

            var details = new ProblemDetails()
            {
                Detail = $"API Error {exception.Message}",
                Instance = "API ",
                Status = (int) HttpStatusCode.InternalServerError,
                Title= "Api Error",
                Type = "Server Error"
            };
            var response = JsonSerializer.Serialize(details);
            httpContext.Response.ContentType= "application/json";
            await httpContext.Response.WriteAsync(response,token);
            
            return true;
        }
    }
}