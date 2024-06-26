﻿namespace PrisonersDilemma.Runner;

public class ScoreBoard
{
    private readonly IDictionary<string, int> _scores = new Dictionary<string, int>();
    public int GameCount { get; private set; }

    public void Add(IStrategy strategy, int score)
    {
        if (!_scores.TryGetValue(strategy.GetName(), out var total))
        {
            _scores.Add(strategy.GetName(), 0);
        }

        _scores[strategy.GetName()] += score;
        GameCount++;
    }

    public IEnumerable<GameResult> GetResults()
    {
        foreach (var score in _scores)
        {
            yield return new GameResult(score.Key, score.Value);
        }
    }
}

public record GameResult(string StrategyName, int Score);