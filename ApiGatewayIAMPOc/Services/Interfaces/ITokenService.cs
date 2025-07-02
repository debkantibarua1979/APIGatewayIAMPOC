namespace ApiGatewayIAMPOc.Services.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(string userId); 
    string GenerateRefreshToken(); 
}
