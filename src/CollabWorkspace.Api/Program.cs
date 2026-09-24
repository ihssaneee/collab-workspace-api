// ============================================================
// Imports
// ============================================================

using CollabWorkspace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using CollabWorkspace.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CollabWorkspace.Infrastructure.Authentication;


var builder = WebApplication.CreateBuilder(args);


// ============================================================
// API / Swagger
// ============================================================

// Register controller support so ASP.NET Core can discover controllers.
builder.Services.AddControllers();

// Register services needed by Swagger/OpenAPI.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// ============================================================
// Database
// ============================================================

// Register EF Core DbContext.
// The connection string is read from application configuration / User Secrets.
builder.Services.AddDbContext<CollabWorkspaceDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));


// ============================================================
// ASP.NET Core Identity
// ============================================================

// Register Identity for user management:
// - creating users
// - password hashing and verification
// - user/account management
// - roles
//
// Identity stores its data through our EF Core DbContext.
builder.Services.AddIdentityCore<ApplicationUser>()
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<CollabWorkspaceDbContext>();


// ============================================================
// JWT Configuration
// ============================================================

// Bind the "Authentication:Jwt" configuration section
// to the JwtOptions class so we can inject JWT settings.
builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection("Authentication:Jwt"));


// ============================================================
// Authentication
// ============================================================

// Tell ASP.NET Core that JWT Bearer tokens are the authentication
// mechanism used to identify users making API requests.
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Keep JWT claim names as they appear in the token
        // instead of mapping them to older .NET claim names.
        options.MapInboundClaims = false;

        // Rules used to decide whether an incoming JWT is valid.
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Token must have our expected issuer.
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Authentication:Jwt:Issuer"],

            // Token must have our expected audience.
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Authentication:Jwt:Audience"],

            // Reject expired tokens.
            ValidateLifetime = true,

            // Verify that the token was signed with our trusted key.
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["Authentication:Jwt:Secret"]!)),

            // Only accept tokens signed using HMAC-SHA256.
            ValidAlgorithms = new[]
            {
                SecurityAlgorithms.HmacSha256
            }
        };
    });


// ============================================================
// Authorization
// ============================================================

// Enables authorization checks such as [Authorize]
// after authentication has identified the user.
builder.Services.AddAuthorization();


// ============================================================
// Application Services
// ============================================================

// Register our JWT token service with dependency injection.
// Whenever ITokenService is requested, provide JwtTokenService.
builder.Services.AddScoped<ITokenService, JwtTokenService>();


var app = builder.Build();


// ============================================================
// HTTP Request Pipeline
// ============================================================

// Swagger is enabled only during development.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirect HTTP requests to HTTPS.
app.UseHttpsRedirection();

// Authenticate the request and build HttpContext.User.
app.UseAuthentication();

// Check whether the authenticated user is allowed
// to access the requested resource.
app.UseAuthorization();

// Connect controller routes to the HTTP pipeline.
app.MapControllers();


// Start the application.
app.Run();