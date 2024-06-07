using Microsoft.Extensions.DependencyInjection;

namespace PrisonersDilemma.Runner;

public static class StrategyInstaller
{
    public static IServiceCollection AddStrategies(this IServiceCollection services)
    {
        var strategyTypeProvider = new StrategyTypeProvider();
        services.AddSingleton<IStrategyTypeProvider>(strategyTypeProvider);
        services.AddSingleton<IStrategyFactory, ServiceProviderStrategyFactory>();

        var strategies = strategyTypeProvider.GetStrategyTypes();
        foreach (var strategyType in strategies)
        {
            services.AddTransient(strategyType);
        }
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<GameRunner>();
        return services;
    }
}