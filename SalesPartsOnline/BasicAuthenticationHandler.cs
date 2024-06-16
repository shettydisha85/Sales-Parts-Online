using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
            return AuthenticateResult.Fail("Missing Authorization Header");

        var authHeader = Request.Headers["Authorization"].ToString();
        if (authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
        {
            var token = authHeader.Substring("Basic ".Length).Trim();
            var credentialString = Encoding.UTF8.GetString(Convert.FromBase64String(token));
            var credentials = credentialString.Split(':');

            if (credentials.Length == 2)
            {
                var username = credentials[0];
                var password = credentials[1];

                // Validate the credentials (this is just an example, in a real scenario you should validate from a database)
                if (username == "test" && password == "password")
                {
                    var claims = new[] {
                        new Claim(ClaimTypes.NameIdentifier, username),
                        new Claim(ClaimTypes.Name, username)
                    };

                    var identity = new ClaimsIdentity(claims, Scheme.Name);
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);

                    return AuthenticateResult.Success(ticket);
                }
            }
        }

        return AuthenticateResult.Fail("Invalid Authorization Header");
    }
}