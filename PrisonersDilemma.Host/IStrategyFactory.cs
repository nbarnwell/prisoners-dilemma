namespace PrisonersDilemma.Runner;

public interface IStrategyFactory
{
    IStrategy Create(Type strategyType);
}