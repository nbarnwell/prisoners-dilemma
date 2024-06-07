namespace PrisonersDilemma.Runner;

public class ServiceProviderStrategyFactory : IStrategyFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ServiceProviderStrategyFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public IStrategy Create(Type strategyType)
    {
        return (IStrategy)_serviceProvider.GetService(strategyType) ?? throw new InvalidOperationException($"No strategy registered for type {strategyType}");
    }
}