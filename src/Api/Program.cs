using Api;
using Content.Application;
using Content.Infrastructure;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<TestDbContext>(o => o.UseNpgsql(connectionString));
builder.Services.AddScoped<ITestElementRepository, EFTestElementRepository>();
builder.Services.AddScoped<IUnitOfWork, EFUnitOfWork>();
builder.Services.AddWriteUseCase<AddElementUseCase, AddElementInput, VoidOutput>();
builder.Services.AddWriteUseCase<RemoveElementUseCase, int, VoidOutput>();
builder.Services.AddScoped<GetElementsUseCase>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/tests", async (GetElementsUseCase useCase, CancellationToken ct) => (await useCase.ExecuteAsync(new(), ct)).Content);

app.Run();
