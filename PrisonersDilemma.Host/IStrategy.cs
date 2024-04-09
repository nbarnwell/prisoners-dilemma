namespace PrisonersDilemma.Runner;

public interface IStrategy
{
    Task<Decision> Decide(Decision? previousOpponentDecision);
    string GetName();
}