using PrisonersDilemma.Runner;
using PrisonersDilemma.Runner.Strategies;

namespace PrisonersDilemma.Tests;

public class TestStrategyFactory : IStrategyFactory
{
    public IStrategy Create(Type strategyType)
    {
        if (strategyType == typeof(Nice))
        {
            return new Nice();
        }

        if (strategyType == typeof(Nasty))
        {
            return new Nasty();
        }

        throw new NotImplementedException($"No test support for {strategyType}");
    }
}