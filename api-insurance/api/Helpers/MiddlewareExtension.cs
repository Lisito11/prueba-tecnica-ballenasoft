using System;
using api.Middlewares;

namespace api.Helpers
{
	public static class MiddlewareExtension
    {
        public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder builder) => builder.UseMiddleware<AuthMiddleware>();
    }
}

