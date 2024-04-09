namespace PrisonersDilemma.Runner.Strategies;

public class Nice : IStrategy
{
    public Task<Decision> Decide(Decision? previousOpponentDecision)
    {
        return Task.FromResult(Decision.Cooperate);
    }

    public string GetName()
    {
        return GetType().Name;
    }
}