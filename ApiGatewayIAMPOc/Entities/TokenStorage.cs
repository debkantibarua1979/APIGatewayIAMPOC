namespace ApiGatewayIAMPOc.Entities;

public class TokenStorage
{
    public Guid Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
}