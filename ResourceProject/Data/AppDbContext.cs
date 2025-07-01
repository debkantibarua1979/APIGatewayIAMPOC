using Microsoft.EntityFrameworkCore;
using ResourceProject.Entities;
using ResourceProject.Entities.JoinEntities;
using Task = ResourceProject.Entities.Task;


namespace ResourceProject.Data;

public class AppDbContext: DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Task> Tasks { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Task>()
            .HasOne(t => t.Project) // A Task has one Project
            .WithMany(p => p.Tasks) // A Project has many Tasks
            .HasForeignKey(t => t.ProjectId); // Foreign key for ProjectId in Task

        // One-to-many relationship between User and Role
        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId);

        // Many-to-many relationship between Role and RolePermission via the RolePermissionRole join entity
        modelBuilder.Entity<Role>()
            .HasMany(r => r.RolePermissionRoles)  // Role has many RolePermissionRole
            .WithOne(rpr => rpr.Role)             // Each RolePermissionRole links to one Role
            .HasForeignKey(rpr => rpr.RoleId);    // Foreign key for RoleId

        modelBuilder.Entity<RolePermission>()
            .HasMany(rp => rp.RolePermissionRoles)  // RolePermission has many RolePermissionRole
            .WithOne(rpr => rpr.RolePermission)     // Each RolePermissionRole links to one RolePermission
            .HasForeignKey(rpr => rpr.PermissionId); // Foreign key for PermissionId

        // Define composite keys for the join entity RolePermissionRole
        modelBuilder.Entity<RolePermissionRole>().HasKey(rp => new { rp.RoleId, rp.PermissionId });
    }



}