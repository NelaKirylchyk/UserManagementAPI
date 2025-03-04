using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System;
namespace UserManagementAPI.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;
            var requestLog = $"HTTP {request.Method} {request.Path}";

            // Log request details
            Console.WriteLine(requestLog);

            // Copy original response body stream to capture the response
            var originalBodyStream = context.Response.Body;
            using var responseBodyStream = new MemoryStream();
            context.Response.Body = responseBodyStream;

            await _next(context);

            // Log response details
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseLog = $"Response: {context.Response.StatusCode} {responseBodyText}";
            Console.WriteLine(responseLog);

            // Copy the response back to the original stream
            await responseBodyStream.CopyToAsync(originalBodyStream);
        }
    }

}
