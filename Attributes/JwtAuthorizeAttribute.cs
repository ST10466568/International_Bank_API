using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HopewellClinicApi.Attributes
{
    public class JwtAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                var authHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                {
                    context.Result = new UnauthorizedObjectResult(new { message = "No valid authorization header found" });
                    return;
                }

                var token = authHeader.Substring("Bearer ".Length).Trim();
                
                // Get JWT settings from configuration
                var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
                var jwtSettings = configuration.GetSection("JwtSettings");
                var secretKey = jwtSettings["SecretKey"];
                var issuer = jwtSettings["Issuer"];
                var audience = jwtSettings["Audience"];

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
                var tokenHandler = new JwtSecurityTokenHandler();

                try
                {
                    var validationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateIssuer = true,
                        ValidIssuer = issuer,
                        ValidateAudience = true,
                        ValidAudience = audience,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };

                    var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                    
                    // Set the user principal for the request
                    context.HttpContext.User = principal;
                }
                catch (SecurityTokenException ex)
                {
                    context.Result = new UnauthorizedObjectResult(new { message = "Token validation failed", error = ex.Message });
                    return;
                }
            }
            catch (Exception ex)
            {
                context.Result = new StatusCodeResult(500);
                return;
            }
        }
    }
}



