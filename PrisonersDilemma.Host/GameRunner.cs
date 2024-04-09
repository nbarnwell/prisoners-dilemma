using System.Reflection;
using Microsoft.Extensions.Logging;

namespace PrisonersDilemma.Runner;

public class GameRunner
{
    private readonly IServiceProvider _services;
    private readonly ILogger<GameRunner> _logger;

    public GameRunner(IServiceProvider services, ILogger<GameRunner> logger)
    {
        _services = services ?? throw new ArgumentNullException(nameof(services));
        _logger   = logger   ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Run()
    {
        // Get all combinations of all strategies and play each against each other 200 times
        var strategyTypes =
            Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(x => x is { IsAbstract: false, IsClass: true })
                    .Where(x => typeof(IStrategy).IsAssignableFrom(x));

        var pairs = strategyTypes.SelectMany(
                                     x => strategyTypes,
                                     (player, opponent) => new { Player = player, Opponent = opponent })
                                 .Select(x => new
                                 {
                                     Player = (IStrategy)_services.GetService(x.Player),
                                     Opponent = (IStrategy)_services.GetService(x.Opponent)
                                 });

        foreach (var pair in pairs)
        {
            await Play(pair.Player, pair.Opponent);
        }
    }

    private async Task Play(IStrategy player, IStrategy opponent)
    {
        _logger.LogInformation($"Playing {player.GetType().Name} against {opponent.GetType().Name}...");
        Decision? playerDecision = null;
        Decision? opponentDecision = null;
        for (int i = 0; i < 200; i++)
        {
            playerDecision = await player.Decide(opponentDecision);
            opponentDecision = await opponent.Decide(playerDecision);

            var score = Score.From(playerDecision.Value, opponentDecision.Value);

            _logger.LogInformation($"{player.GetType().Name} vs. {opponent.GetType().Name} {score.PlayerScore}/{score.OpponentScore}");
        }
    }
}

internal class Score
{
    public readonly int PlayerScore;
    public readonly int OpponentScore;

    public Score(int playerScore, int opponentScore)
    {
        PlayerScore        = playerScore;
        OpponentScore = opponentScore;
    }

    public static Score From(Decision playerDecision, Decision opponentDecision)
    {
        switch (playerDecision)
        {
            case Decision.Cooperate when opponentDecision == Decision.Cooperate:
                return new Score(3, 3);
            case Decision.Cooperate when opponentDecision == Decision.Defect:
                return new Score(5, 0);
            case Decision.Defect when opponentDecision == Decision.Cooperate:
                return new Score(0, 5);
            case Decision.Defect when opponentDecision == Decision.Defect:
                return new Score(1, 1);
            default:
                throw new ArgumentOutOfRangeException(nameof(playerDecision), playerDecision, null);
        }
    }
}