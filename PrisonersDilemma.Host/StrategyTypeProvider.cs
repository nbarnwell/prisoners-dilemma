using System.Reflection;

namespace PrisonersDilemma.Runner;

public class StrategyTypeProvider : IStrategyTypeProvider
{
    public IEnumerable<Type> GetStrategyTypes()
    {
        return
            Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(x => x is { IsAbstract: false, IsClass: true })
                    .Where(x => typeof(IStrategy).IsAssignableFrom(x));
    }
}