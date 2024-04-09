using Microsoft.Extensions.Logging;

namespace PrisonersDilemma.Runner;

public class GameRunner
{
    private readonly IStrategyFactory _strategyFactory;
    private readonly ILogger<GameRunner> _logger;

    public GameRunner(IStrategyFactory strategyFactory, ILogger<GameRunner> logger)
    {
        _strategyFactory = strategyFactory ?? throw new ArgumentNullException(nameof(strategyFactory));
        _logger          = logger          ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ScoreBoard> Run()
    {
        var scoreboard = new ScoreBoard();

        // Get all combinations of all strategies and play each against each other 200 times
        var strategyTypes = StrategyTypeProvider.GetStrategyTypes();

        var pairs = strategyTypes.SelectMany(
                                     _ => strategyTypes,
                                     (player, opponent) => new { PlayerType = player, OpponentType = opponent })
                                 .Select(x => new
                                 {
                                     Player   = _strategyFactory.Create(x.PlayerType),
                                     Opponent = _strategyFactory.Create(x.OpponentType)
                                 });

        foreach (var pair in pairs)
        {
            var scores = await Play(pair.Player, pair.Opponent);

            foreach (var score in scores)
            {
                scoreboard.Add(pair.Player, score.PlayerScore);
                scoreboard.Add(pair.Opponent, score.OpponentScore);
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