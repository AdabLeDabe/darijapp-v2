using Api;
using Content.Application;
using Content.Infrastructure;
using Identity.Application;
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

// Content
builder.Services.AddDbContext<TestDbContext>(o => o.UseNpgsql(connectionString));
builder.Services.AddScoped<ITestElementRepository, EfTestElementRepository>();
builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();
builder.Services.AddWriteUseCase<AddElementUseCase, AddElementInput, VoidOutput>();
builder.Services.AddWriteUseCase<RemoveElementUseCase, int, VoidOutput>();
builder.Services.AddScoped<GetElementsUseCase>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/tests", async (GetElementsUseCase useCase, CancellationToken ct) => (await useCase.ExecuteAsync(new(), ct)).Content);

app.Run();
