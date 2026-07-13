using Application.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<WorldRankDbContext>(options =>
            options.UseSqlServer("Server=localhost;Database=WorldRank;Integrated Security=true;TrustServerCertificate=true"));

        services.AddSingleton<IPlayerRepository, InMemoryPlayerRepository>();
        services.AddSingleton<IWalletRepository, InMemoryWalletRepository>();

        return services;
    }
}
