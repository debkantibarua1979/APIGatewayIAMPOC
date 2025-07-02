using Microsoft.AspNetCore.Identity;

namespace ApiGatewayIAMPOc.Entities;

public class ApplicationUser: IdentityUser
{
    public string FullName { get; set; }
    public string Designation { get; set; }
    public string Name { get; set; }
}