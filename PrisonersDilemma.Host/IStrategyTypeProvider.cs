namespace PrisonersDilemma.Runner;

public interface IStrategyTypeProvider
{
    IEnumerable<Type> GetStrategyTypes();
}