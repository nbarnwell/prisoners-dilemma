using System.Reflection;

namespace PrisonersDilemma.Runner;

public static class StrategyTypeProvider
{
    public static IEnumerable<Type> GetStrategyTypes()
    {
        return
            Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(x => x is { IsAbstract: false, IsClass: true })
                    .Where(x => typeof(IStrategy).IsAssignableFrom(x));
    }
}