using Api;
using Content.Application;
using Content.Infrastructure;
using Identity.Application;
using Identity.Application.Register;
using Identity.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Default");

// Identity
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

app.MapGet("/", () => "Hello World!");
app.MapGet("/tests", async (GetElementsUseCase useCase, CancellationToken ct) => (await useCase.ExecuteAsync(new(), ct)).Content);

app.Run();
