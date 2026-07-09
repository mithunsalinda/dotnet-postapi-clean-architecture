using Microsoft.EntityFrameworkCore;
using PostApi.Application.Interfaces;
using PostApi.Application.Services;
using PostApi.Extensions;
using PostApi.Infrastructure.Data;
using PostApi.Infrastructure.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add controllers
builder.Services.AddControllers();

// Add OpenAPI
builder.Services.AddOpenApiDocumentation();

// Add PostgreSQL DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    );
});

// Add JWT Authentication
builder.Services.AddJwtAuthentication(builder.Configuration);

// Add Application Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

// Add UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Enable OpenAPI in development
app.UseOpenApiDocumentation();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();