using ApiGatewayIAMPOc.Entities;

namespace ApiGatewayIAMPOc.Services.Interfaces;

using System.Threading.Tasks;

public interface IAuthService
{
    Task<string> GenerateAndStoreRefreshTokenAsync(ApplicationUser user);
    Task<bool> RevokeRefreshTokenAsync(string token);
    Task RemoveExpiredTokensAsync(string userId);
    Task<string> GenerateAccessTokenAsync(ApplicationUser user);
    Task<ApplicationUser> ValidateUserCredentialsAsync(string username, string password);
}