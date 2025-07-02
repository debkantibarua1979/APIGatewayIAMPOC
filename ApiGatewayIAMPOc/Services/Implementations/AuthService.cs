using ApiGatewayIAMPOc.Entities;
using ApiGatewayIAMPOc.Repositories.Interfaces;
using ApiGatewayIAMPOc.Services.Interfaces;


namespace ApiGatewayIAMPOc.Services.Implementations;

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

public class AuthService : IAuthService
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ITokenService _tokenService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthService(
        IRefreshTokenRepository refreshTokenRepository,
        ITokenService tokenService,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _tokenService = tokenService;
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    // Generate and store refresh token for the user
    public async Task<string> GenerateAndStoreRefreshTokenAsync(ApplicationUser user)
    {
        var refreshToken = new RefreshToken
        {
            Token = _tokenService.GenerateRefreshToken(),
            ExpirationDate = DateTime.UtcNow.AddDays(7),
            UserId = user.Id,
            IsRevoked = false,
            IsUsed = false
        };

        await _refreshTokenRepository.AddRefreshTokenAsync(refreshToken);
        return refreshToken.Token;
    }

    // Revoke a refresh token (mark it as revoked)
    public async Task<bool> RevokeRefreshTokenAsync(string token)
    {
        return await _refreshTokenRepository.RevokeRefreshTokenAsync(token);
    }

    // Remove expired or used refresh tokens for a given user
    public async Task RemoveExpiredTokensAsync(string userId)
    {
        await _refreshTokenRepository.RemoveUsedOrExpiredTokensAsync(userId);
    }

    // Generate an access token for the user
    public async Task<string> GenerateAccessTokenAsync(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("userId", user.Id.ToString())
        };

        // Add any additional claims as needed, for example roles
        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30), // Token expiry time
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // Validate user credentials (username and password)
    public async Task<ApplicationUser> ValidateUserCredentialsAsync(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null) return null;

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        if (result.Succeeded)
        {
            return user;
        }

        return null;
    }
}