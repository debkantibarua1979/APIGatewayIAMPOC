using ApiGatewayIAMPOc.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiGatewayIAMPOc.Data;

public class AppDbContext: IdentityDbContext<ApplicationUser>
{
    public DbSet<TokenStorage> TokenStorages { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Example of token storage with IP-based restriction
        modelBuilder.Entity<TokenStorage>()
            .HasIndex(t => t.Token)
            .IsUnique();
    }
}