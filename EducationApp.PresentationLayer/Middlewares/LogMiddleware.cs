using EducationApp.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EducationApp.PresentationLayer.Middlewares
{
    public class LogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        public LogMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            this._next = next;
            _logger = loggerFactory.CreateLogger<LogMiddleware>();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (CustomApiException exception)
            {
                context.Response.StatusCode = (int)exception.Status;
                _logger.LogInformation($"\n{DateTime.Now}: {(int)exception.Status} - {exception.Message}\n");
                await context.Response.WriteAsync(exception.Message);
            }
            finally
            {
                _logger.LogInformation(
                    "\nRequest {method} {url} => {statusCode}\n",
                    context.Request?.Method,
                    context.Request?.Path.Value,
                    context.Response?.StatusCode);
            }
        }
    }
}