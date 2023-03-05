using System;
using System.Net;
using Microsoft.Extensions.Primitives;

namespace api.Middlewares
{
	public class AuthMiddleware
	{
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            string token = context.Request.Headers["token"]!;
            if (string.IsNullOrEmpty(token) || token != "1234")
            {
                await ReturnErrorResponse(context);
            }
            else
            {
                await _next(context);
            }
        }

        private static async Task ReturnErrorResponse(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
            await context.Response.StartAsync();
        }
    }

}

