using ApiGatewayIAMPOc.Services.Interfaces;

namespace ApiGatewayIAMPOc.Services.Implementations;

using System;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    
    public string GenerateAccessToken(string userId)
    {
        return "generated_access_token"; 
    }

    // Generate Refresh Token
    public string GenerateRefreshToken()
    {
        
        var randomNumber = new byte[32]; 
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
        }
        
        return Convert.ToBase64String(randomNumber);
    }
}
