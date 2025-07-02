using Microsoft.EntityFrameworkCore;
using ResourceProject.Data;
using ResourceProject.Repositories;
using ResourceProject.Repositories.Implementations;
using ResourceProject.Repositories.Interfaces;
using ResourceProject.Resolvers.Mutations;
using ResourceProject.Resolvers.Queries;
using ResourceProject.Services.Implementations;
using ResourceProject.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add Entity Framework and DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Add services for Role, User, and RolePermission
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IRolePermissionService, RolePermissionService>();

// Add GraphQL services
builder.Services
    .AddGraphQLServer()
    .AddType<UserQuery>() 
    .AddType<UserMutation>() 
    .AddType<RoleQuery>() 
    .AddType<RoleMutation>() 
    .AddType<RolePermissionQuery>() 
    .AddType<RolePermissionMutation>() 
    .AddAuthorization() 
    .AddInMemorySubscriptions(); 

builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();

builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectService, ProjectService>();

builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();

builder.Services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
builder.Services.AddScoped<IRolePermissionService, RolePermissionService>();


builder.Services.AddAuthorization(options =>
{
    // Automatically add policies based on permissions in the database
    using (var scope = builder.Services.BuildServiceProvider().CreateScope())
    {
        var rolePermissionService = scope.ServiceProvider.GetRequiredService<IRolePermissionService>();
        var permissions = rolePermissionService.GetAllRolePermissions().Result;

        foreach (var permission in permissions)
        {
            options.AddPolicy(permission.ToString(), policy =>
                policy.RequireClaim("permission", permission.ToString()));
        }
    }
});


// Add Swagger for API documentation (optional, you can remove it if not needed)
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Description = "Please enter 'Bearer' followed by your token."
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


var app = builder.Build();

// Middleware configuration
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Enable Swagger UI (optional)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
});

// Authentication and Authorization Middleware
app.UseAuthentication();
app.UseAuthorization();


// Enable GraphQL Server
app.MapGraphQL();

// Map default controllers if any
app.MapControllers();

// Run the application
app.Run();
