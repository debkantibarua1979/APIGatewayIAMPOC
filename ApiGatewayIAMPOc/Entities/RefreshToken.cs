namespace ApiGatewayIAMPOc.Entities;

using System;

public class RefreshToken
{
    public string Token { get; set; } 
    public DateTime ExpirationDate { get; set; } 
    public string UserId { get; set; } 
    public bool IsRevoked { get; set; } 
    public bool IsUsed { get; set; } 

    // Navigation property to User
    public ApplicationUser User { get; set; } 
}