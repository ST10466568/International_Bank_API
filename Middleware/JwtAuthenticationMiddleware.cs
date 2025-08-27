using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace HopewellClinicApi.Middleware
{
    public class JwtAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public JwtAuthenticationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Skip authentication for public endpoints
                if (IsPublicEndpoint(context.Request.Path))
                {
                    await _next(context);
                    return;
                }

                // Check if endpoint requires authentication
                if (RequiresAuthentication(context.Request.Path))
                {
                    var token = ExtractToken(context.Request);
                    
                    if (string.IsNullOrEmpty(token))
                    {
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync("{\"error\": \"JWT token required\"}");
                        return;
                    }

                    if (!ValidateToken(token))
                    {
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync("{\"error\": \"Invalid JWT token\"}");
                        return;
                    }

                    // Token is valid, continue
                    await _next(context);
                    return;
                }

                // Endpoint doesn't require authentication
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log error and continue without authentication
                Console.WriteLine($"Authentication middleware error: {ex.Message}");
                await _next(context);
            }
        }

        private bool IsPublicEndpoint(PathString path)
        {
            var publicEndpoints = new[]
            {
                "/WeatherForecast",
                "/api/test",
                "/api/test/unprotected",
                "/swagger",
                "/swagger-ui"
            };

            return publicEndpoints.Any(endpoint => path.StartsWithSegments(endpoint));
        }

        private bool RequiresAuthentication(PathString path)
        {
            var protectedEndpoints = new[]
            {
                "/api/test/protected",
                "/api/services",
                "/api/appointments",
                "/api/staff"
            };

            return protectedEndpoints.Any(endpoint => path.StartsWithSegments(endpoint));
        }

        private string? ExtractToken(HttpRequest request)
        {
            var authHeader = request.Headers["Authorization"].FirstOrDefault();
            
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                return null;

            return authHeader.Substring("Bearer ".Length);
        }

        private bool ValidateToken(string token)
        {
            try
            {
                var secretKey = _configuration["JwtSettings:SecretKey"];
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(secretKey!);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                return principal != null;
            }
            catch
            {
                return false;
            }
        }
    }

    public static class JwtAuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtAuthenticationMiddleware>();
        }
    }
}
