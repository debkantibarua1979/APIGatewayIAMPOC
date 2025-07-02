namespace ApiGatewayIAMPOc.Middlewares;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

public class TokenExpiryMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TokenExpiryMiddleware> _logger;

    public TokenExpiryMiddleware(RequestDelegate next, ILogger<TokenExpiryMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token != null)
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
            if (jwtToken != null && jwtToken.ValidTo < DateTime.UtcNow)
            {
                _logger.LogWarning("Token expired");
                httpContext.Response.StatusCode = 401; // Unauthorized
                await httpContext.Response.WriteAsync("Token expired");
                return;
            }
        }

        await _next(httpContext);
    }
}