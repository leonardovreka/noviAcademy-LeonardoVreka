using Application.Interfaces;
using Application.Services;
using Application.Strategies;
using Infrastructure;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using NLog.Extensions.Logging;
using System.Text.Json.Serialization;
using Autofac;
using Autofac.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

// Logging via NLog (same nlog.config layout as the Console app).
builder.Logging.ClearProviders();
builder.Logging.AddNLog("nlog.config");

builder.Services.AddDbContext<WorldRankDbContext>(options => {
    options.UseSqlServer("Server=localhost;Database=WorldRank;Integrated Security=true; TrustServerCertificate=true");
});

//Services
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IWalletService, WalletService>();

//strategy
//builder.Services.AddSingleton<IFundsStrategy, AddFundsStrategy>();
//builder.Services.AddSingleton<IFundsStrategy, SubtractFundsStrategy>();
//builder.Services.AddSingleton<IFundsStrategy, ForceSubtractFundsStrategy>();

// Single-instance in-memory cache (Day 6). Redis would replace this behind a load balancer.
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ICache, MemoryCacheStore>();

// Accept/emit enums (e.g. Currency) as their string names, not numbers.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Swagger / OpenAPI — interactive API docs at /swagger.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule<Application.ApplicationModule>();
    containerBuilder.RegisterModule<Infrastructure.InfrastructureModule>();
});

var app = builder.Build();

// Serve the Swagger JSON and UI in Development.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapGet("/", () => Results.Redirect("/swagger")); // root → Swagger UI
}

app.MapControllers();

app.Run();