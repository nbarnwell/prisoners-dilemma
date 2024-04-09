using Microsoft.Extensions.DependencyInjection;

namespace PrisonersDilemma.Runner;

public static class StrategyInstaller
{
    public static IServiceCollection AddStrategies(this IServiceCollection services)
    {
        services.AddTransient<Nice>();
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<GameRunner>();
        return services;
    }
}