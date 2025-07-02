using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiGatewayIAMPOc.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ResourceProject.Entities;

namespace ApiGatewayIAMPOc.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly RoleManager<Role> _roleManager;

    public AuthController(
        UserManager<ApplicationUser> userManager, 
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration,
        RoleManager<Role> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _roleManager = roleManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationDto registrationDto)
    {
        var user = new ApplicationUser
        {
            UserName = registrationDto.Username,
            Email = registrationDto.Email,
            Name = registrationDto.Name
        };

        var result = await _userManager.CreateAsync(user, registrationDto.Password);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok("Registration successful!");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var user = await _userManager.FindByNameAsync(loginDto.Username);
        if (user == null || !(await _userManager.CheckPasswordAsync(user, loginDto.Password)))
        {
            return Unauthorized("Invalid credentials");
        }

        // Get role and permissions
        var roles = await _userManager.GetRolesAsync(user);
        var permissions = new List<string>();
        
        foreach (var role in roles)
        {
            var roleEntity = await _roleManager.FindByNameAsync(role);
            if (roleEntity != null)
            {
                permissions.AddRange(roleEntity.RolePermissionRoles.Select(rpr => rpr.RolePermission.PermissionName));
            }
        }

        // Generate JWT token
        var token = GenerateJwtToken(user, roles, permissions);
        return Ok(new { Token = token });
    }

    private string GenerateJwtToken(ApplicationUser user, IList<string> roles, List<string> permissions)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
        };

        // Add role claims
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        // Add permissions as claims
        foreach (var permission in permissions)
        {
            claims.Add(new Claim("permission", permission));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}