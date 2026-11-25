using System.Security.Claims;
using System.Text;
using Api;
using Content.Application;
using Content.Infrastructure;
using Identity.Application;
using Identity.Application.Login;
using Identity.Application.Register;
using Identity.Infrastructure;
using Identity.Infrastructure.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SharedKernel;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Default");

// Identity - Registration
builder.Services.AddDbContext<AuthDbContext>(o =>
    o.UseNpgsql(connectionString, npgsql => npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "identity")));
builder.Services.AddIdentityCore<AppUser>(o =>
    {
        o.User.RequireUniqueEmail = true;
        o.Password.RequiredLength = 8;
        o.Password.RequireNonAlphanumeric = false;
    })
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AuthDbContext>();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddScoped<RegisterUseCase>();

// Identity - Login
var jwtSection = builder.Configuration.GetSection("Jwt");
builder.Services.Configure<JwtOptions>(jwtSection);
var jwt = jwtSection.Get<JwtOptions>();
if (string.IsNullOrWhiteSpace(jwt?.Key))
    throw new InvalidOperationException("No key was found for JWT, please configure one");

builder.Services.AddScoped<ITokenIssuer, JwtTokenIssuer>();
builder.Services.AddScoped<LoginUseCase>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.MapInboundClaims = false;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = jwt.Issuer,
            ValidAudience = jwt.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
            NameClaimType = "sub",
            RoleClaimType = "role",
            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });
builder.Services.AddAuthorization();

// Content
builder.Services.AddDbContext<TestDbContext>(o => o.UseNpgsql(connectionString));
builder.Services.AddScoped<ITestElementRepository, EfTestElementRepository>();
builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();
builder.Services.AddWriteUseCase<AddElementUseCase, AddElementInput, VoidOutput>();
builder.Services.AddWriteUseCase<RemoveElementUseCase, int, VoidOutput>();
builder.Services.AddScoped<GetElementsUseCase>();

var app = builder.Build();

// Init identity roles
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
    foreach (var role in new[] { Roles.Learner, Roles.Admin })
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole<Guid>(role));
    }
}

var auth = app.MapGroup("/api/auth");
auth.MapPost("/register", async (RegisterInput input, RegisterUseCase useCase, CancellationToken ct) =>
{
    var result = await useCase.ExecuteAsync(input, ct);
    return result.Status == ResultStatus.Success
        ? Results.Ok(new { id = result.Content })
        : Results.BadRequest();
});

// Init identity login
app.UseAuthentication();
app.UseAuthorization();

auth.MapPost("/login", async (LoginInput input, LoginUseCase useCase, CancellationToken ct) =>
{
    var result = await useCase.ExecuteAsync(input, ct);
    return result.Status == ResultStatus.Success
        ? Results.Ok(result.Content)
        : Results.Unauthorized();
});
auth.MapGet("/me", (ClaimsPrincipal user) => Results.Ok(
    new
    {
        id = user.FindFirstValue("sub"),
        email = user.FindFirstValue("email"),
        roles = user.FindAll("role").Select(c => c.Value)
    })).RequireAuthorization();

app.MapGet("/", () => "Hello World!");
app.MapGet("/tests", async (GetElementsUseCase useCase, CancellationToken ct) => (await useCase.ExecuteAsync(new(), ct)).Content);

app.Run();
