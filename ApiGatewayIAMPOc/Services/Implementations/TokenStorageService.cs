using ApiGatewayIAMPOc.Data;
using ApiGatewayIAMPOc.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiGatewayIAMPOc.Services.Implementations;

public class TokenStorageService
{
    private readonly AppDbContext _context;

    public TokenStorageService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> StoreTokenAsync(string token, string ipAddress, DateTime expiration)
    {
        var tokenStorage = new TokenStorage
        {
            Token = token,
            IpAddress = ipAddress,
            Expiration = expiration
        };

        _context.TokenStorages.Add(tokenStorage);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsTokenValidAsync(string token, string ipAddress)
    {
        var tokenRecord = await _context.TokenStorages
            .FirstOrDefaultAsync(t => t.Token == token && t.IpAddress == ipAddress);

        if (tokenRecord == null || tokenRecord.Expiration < DateTime.UtcNow)
        {
            return false;  // Token not found or expired
        }

        return true;
    }
}