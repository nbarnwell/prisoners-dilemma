using Microsoft.Extensions.DependencyInjection;

namespace PrisonersDilemma.Runner;

public static class StrategyInstaller
{
    public static IServiceCollection AddStrategies(this IServiceCollection services)
    {
        var strategies = StrategyTypeProvider.GetStrategyTypes();
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