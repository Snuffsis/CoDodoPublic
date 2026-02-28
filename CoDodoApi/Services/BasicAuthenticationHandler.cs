using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
namespace CoDodoApi.Services;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder) : base(options, logger, encoder) { }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return AuthenticateResult.Fail("Unauthorized");

        }
        string authHeader = Request.Headers.Authorization.ToString();

        if (!authHeader.StartsWith("basic",
                StringComparison.OrdinalIgnoreCase))
            return await Fail();

        string[] credentials = Credentials(authHeader);

        if (credentials[0] != "admin" || credentials[1] != "password")
            return await Fail();

        return await Success(credentials);
    }

    private static string[] Credentials(string authHeader)
    {
        string token = authHeader["Basic ".Length..].Trim();

        return Encoding.UTF8.GetString(Convert.FromBase64String(token))
            .Split(':');
    }

    private async Task<AuthenticateResult> Success(string[] credentials)
    {
        Claim name = new("name", credentials[0]);
        Claim role = new(ClaimTypes.Role, "Admin");

        Claim[] claims = [name, role];

        ClaimsIdentity identity = new(claims, "Basic");
        ClaimsPrincipal claimsPrincipal = new(identity);

        AuthenticationTicket ticket = new(claimsPrincipal, Scheme.Name);
        AuthenticateResult success = AuthenticateResult.Success(ticket);

        return await Task.FromResult(success);
    }

    private async Task<AuthenticateResult> Fail()
    {
        Response.StatusCode = 401;
        Response.Headers.Append("WWW-Authenticate",
            """
            Basic realm="CoDodoApiRealm"
            """);

        return await Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
    }
}