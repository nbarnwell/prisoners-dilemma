using Microsoft.Extensions.Logging;

namespace PrisonersDilemma.Runner;

public class GameRunner
{
    private readonly IStrategyTypeProvider _strategyTypeProvider;
    private readonly IStrategyFactory _strategyFactory;
    private readonly ILogger<GameRunner> _logger;

    public GameRunner(IStrategyTypeProvider strategyTypeProvider, IStrategyFactory strategyFactory, ILogger<GameRunner> logger)
    {
        _strategyTypeProvider = strategyTypeProvider ?? throw new ArgumentNullException(nameof(strategyTypeProvider));
        _strategyFactory      = strategyFactory      ?? throw new ArgumentNullException(nameof(strategyFactory));
        _logger               = logger               ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ScoreBoard> Run()
    {
        var scoreboard = new ScoreBoard();

        // Get all combinations of all strategies and play each against each other 200 times
        var strategyTypes = _strategyTypeProvider.GetStrategyTypes().ToArray();

        var pairs = new List<Tuple<IStrategy, IStrategy>>();

        for (int i = 0; i < strategyTypes.Length; i++)
        {
            pairs.Add(
                Tuple.Create(
                    _strategyFactory.Create(strategyTypes[i]),
                    _strategyFactory.Create(strategyTypes[i])));

            for (int j = i + 1; j < strategyTypes.Length; j++)
            {
                pairs.Add(
                    Tuple.Create(
                        _strategyFactory.Create(strategyTypes[i]),
                        _strategyFactory.Create(strategyTypes[j])));
            }
        }

        foreach (var pair in pairs)
        {
            var scores = await Play(pair.Item1, pair.Item2);

            foreach (var score in scores)
            {
                scoreboard.Add(pair.Item1, score.PlayerScore);
                scoreboard.Add(pair.Item2, score.OpponentScore);
            }
        }

        return scoreboard;
    }

    private async Task<IEnumerable<Score>> Play(IStrategy player, IStrategy opponent)
    {
        _logger.LogInformation($"Playing {player.GetType().Name} against {opponent.GetType().Name}...");

        var scores = new List<Score>();

        Decision? playerPreviousDecision   = null;
        Decision? opponentPreviousDecision = null;
        for (int i = 0; i < 200; i++)
        {
            var playerDecision = await player.Decide(opponentPreviousDecision);
            var opponentDecision = await opponent.Decide(playerPreviousDecision);

            var score = Score.From(playerDecision, opponentDecision);
            playerPreviousDecision = playerDecision;
            opponentPreviousDecision = opponentDecision;

            _logger.LogInformation($"{player.GetType().Name} vs. {opponent.GetType().Name} {score.PlayerScore}/{score.OpponentScore}");

            scores.Add(score);
        }

        return scores;
    }
}