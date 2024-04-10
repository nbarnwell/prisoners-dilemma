using PrisonersDilemma.Runner;
using PrisonersDilemma.Runner.Strategies;

namespace PrisonersDilemma.Tests;

public class TestStrategyTypeProvider : IStrategyTypeProvider
{
    public IEnumerable<Type> GetStrategyTypes()
    {
        yield return typeof(Nice);
    }
}