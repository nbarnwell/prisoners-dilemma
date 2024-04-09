﻿namespace PrisonersDilemma.Runner;

public class Nice : IStrategy
{
    public Task<Decision> Decide(Decision? previousOpponentDecision)
    {
        return Task.FromResult(Decision.Cooperate);
    }
}