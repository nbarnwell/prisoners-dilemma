namespace PrisonersDilemma.Runner.Strategies;

public class Nasty : IStrategy
{
    public Task<Decision> Decide(Decision? previousOpponentDecision)
    {
        return Task.FromResult(Decision.Defect);
    }
}