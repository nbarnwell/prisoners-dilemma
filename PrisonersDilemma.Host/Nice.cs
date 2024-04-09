namespace PrisonersDilemma.Runner;

public class Nice : IStrategy
{
    public async Task<Decision> Decide(Decision? previousOpponentDecision)
    {
        return await Task.FromResult(Decision.Cooperate);
    }
}