using ApiGatewayIAMPOc.Data;
using ApiGatewayIAMPOc.Entities;
using ApiGatewayIAMPOc.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiGatewayIAMPOc.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AppDbContext _context;

    public RefreshTokenRepository(AppDbContext context)
    {
        _context = context;
    }

    // Retrieve a refresh token by the associated user ID
    public async Task<RefreshToken> GetRefreshTokenByUserIdAsync(string userId)
    {
        // Fetch the refresh token for the user that hasn't been used or revoked yet
        return await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.UserId == userId && !rt.IsUsed && !rt.IsRevoked);
    }

    // Retrieve a refresh token by the token string itself
    public async Task<RefreshToken> GetRefreshTokenByTokenAsync(string token)
    {
        // Fetch the refresh token by token value that hasn't been used or revoked
        return await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == token && !rt.IsUsed && !rt.IsRevoked);
    }

    // Add a new refresh token to the database
    public async Task AddRefreshTokenAsync(RefreshToken refreshToken)
    {
        // Add the new refresh token to the context
        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync(); // Persist changes to the database
    }

    // Revoke a refresh token by marking it as revoked
    public async Task<bool> RevokeRefreshTokenAsync(string token)
    {
        // Find the refresh token by token string
        var refreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == token && !rt.IsUsed);

        if (refreshToken == null)
            return false;

        // Mark the token as revoked
        refreshToken.IsRevoked = true;
        _context.RefreshTokens.Update(refreshToken);
        await _context.SaveChangesAsync(); // Save changes to the database
        return true;
    }

    // Remove used or expired tokens
    public async Task<bool> RemoveUsedOrExpiredTokensAsync(string userId)
    {
        // Find all refresh tokens for the user that are either used or expired
        var expiredTokens = await _context.RefreshTokens
            .Where(rt => rt.UserId == userId && (rt.IsUsed || rt.ExpirationDate <= DateTime.UtcNow))
            .ToListAsync();

        if (!expiredTokens.Any())
            return false;

        // Remove the expired or used refresh tokens from the database
        _context.RefreshTokens.RemoveRange(expiredTokens);
        await _context.SaveChangesAsync(); // Persist changes to the database
        return true;
    }
}