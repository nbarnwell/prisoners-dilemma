namespace PrisonersDilemma.Runner;

public class Nasty : IStrategy
{
    public Task<Decision> Decide(Decision? previousOpponentDecision)
    {
        return Task.FromResult(Decision.Defect);
    }
}