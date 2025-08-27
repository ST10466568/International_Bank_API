using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;

namespace HopewellClinicApi.Services
{
    public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "ApiKey";
        public string ApiKeyHeaderName { get; set; } = "X-API-Key";
    }

    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        private readonly IConfiguration _configuration;

        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<ApiKeyAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IConfiguration configuration)
            : base(options, logger, encoder, clock)
        {
            _configuration = configuration;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(Options.ApiKeyHeaderName))
            {
                return Task.FromResult(AuthenticateResult.Fail("API Key header not found."));
            }

            var providedApiKey = Request.Headers[Options.ApiKeyHeaderName].ToString();
            var validApiKey = _configuration["ApiKey"];

            if (string.IsNullOrEmpty(validApiKey))
            {
                Logger.LogWarning("No API key configured in appsettings.json");
                return Task.FromResult(AuthenticateResult.Fail("API key not configured."));
            }

            if (!validApiKey.Equals(providedApiKey))
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid API key."));
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "API User"),
                new Claim(ClaimTypes.Role, "ApiUser"),
                new Claim("ApiKey", "true")
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
