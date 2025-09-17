using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using HopewellClinicApi.Controllers;

namespace HopewellClinicApi.Middleware
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Skip authorization for public endpoints
            var path = context.Request.Path.Value?.ToLower();
            if (IsPublicEndpoint(path))
            {
                await _next(context);
                return;
            }

            // Check for authorization header
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("{\"error\": \"Authorization header required\"}");
                return;
            }

            var token = authHeader.Substring("Bearer ".Length);
            
            // Validate token and set user context
            var session = AuthController.GetSessionFromToken(token);
            if (session == null)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("{\"error\": \"Invalid or expired token\"}");
                return;
            }

            // Set user claims for role-based authorization
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, session.UserId.ToString()),
                new Claim(ClaimTypes.Email, session.Email),
                new Claim(ClaimTypes.Name, session.Email)
            };

            foreach (var role in session.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var identity = new ClaimsIdentity(claims, "Bearer");
            context.User = new ClaimsPrincipal(identity);

            await _next(context);
        }

        private static bool IsPublicEndpoint(string? path)
        {
            if (string.IsNullOrEmpty(path)) return false;

            var publicEndpoints = new[]
            {
                "/api/auth/login",
                "/api/auth/register",
                "/swagger",
                "/health"
            };

            return publicEndpoints.Any(endpoint => path.StartsWith(endpoint));
        }
    }

    // Extension method to register the middleware
    public static class AuthorizationMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomAuthorization(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthorizationMiddleware>();
        }
    }
}
