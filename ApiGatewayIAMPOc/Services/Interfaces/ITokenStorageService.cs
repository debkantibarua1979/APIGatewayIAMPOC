namespace ApiGatewayIAMPOc.Services.Interfaces;

public interface ITokenStorageService
{
    Task<bool> StoreTokenAsync(string token, string ipAddress, DateTime expiration);
    Task<bool> IsTokenValidAsync(string token, string ipAddress);
}
