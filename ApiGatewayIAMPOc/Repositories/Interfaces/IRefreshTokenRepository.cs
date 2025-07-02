using ApiGatewayIAMPOc.Entities;

namespace ApiGatewayIAMPOc.Repositories.Interfaces;

using System;
using System.Threading.Tasks;

public interface IRefreshTokenRepository
{
    Task<RefreshToken> GetRefreshTokenByUserIdAsync(string userId);
    Task<RefreshToken> GetRefreshTokenByTokenAsync(string token);
    Task AddRefreshTokenAsync(RefreshToken refreshToken);
    Task<bool> RevokeRefreshTokenAsync(string token);
    Task<bool> RemoveUsedOrExpiredTokensAsync(string userId);
}