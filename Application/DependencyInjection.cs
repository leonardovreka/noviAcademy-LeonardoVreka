using Application.Strategies;
using Microsoft.Extensions.DependencyInjection;
using Application.Services;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // All strategies are registered under the same interface. The caller resolves
        // them as a collection and picks the one whose Operation matches - no factory.
        services.AddSingleton<IFundsStrategy, AddFundStrategy>();
        services.AddSingleton<IFundsStrategy, SubtractFundsStrategy>();
        services.AddSingleton<IFundsStrategy, ForceSubtractFundsStrategy>();

        // Application services that drive the menu use-cases.
        services.AddSingleton<PlayerService>();
        services.AddSingleton<WalletService>();

        return services;
    }
}