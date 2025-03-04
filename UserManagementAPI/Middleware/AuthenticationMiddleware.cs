using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;

namespace UserManagementAPI.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Allow unauthenticated access to the token generation endpoint
            if (context.Request.Path.StartsWithSegments("/api/token/generate"))
            {
                await _next(context);
                return;
            }

            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            // Validate the token (for demonstration purposes, we're just checking if it's not empty)
            if (string.IsNullOrEmpty(token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            await _next(context);
        }
    }
}
