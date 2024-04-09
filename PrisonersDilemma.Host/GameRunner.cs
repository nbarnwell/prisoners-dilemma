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
        var strategyTypes = StrategyTypeProvider.GetStrategyTypes();

        var pairs = strategyTypes.SelectMany(
                                     _ => strategyTypes,
                                     (player, opponent) => new { Player = player, Opponent = opponent })
                                 .Select(x => new
                                 {
                                     Player   = (IStrategy)_services.GetService(x.Player)!,
                                     Opponent = (IStrategy)_services.GetService(x.Opponent)!
                                 });

        foreach (var pair in pairs)
        {
            await Play(pair.Player, pair.Opponent);
        }
    }

    private async Task Play(IStrategy player, IStrategy opponent)
    {
        _logger.LogInformation($"Playing {player.GetType().Name} against {opponent.GetType().Name}...");
        Decision? playerPreviousDecision = null;
        Decision? opponentPreviousDecision = null;
        for (int i = 0; i < 200; i++)
        {
            var playerDecision = await player.Decide(opponentPreviousDecision);
            var opponentDecision = await opponent.Decide(playerPreviousDecision);

            var score = Score.From(playerDecision, opponentDecision);
            playerPreviousDecision = playerDecision;
            opponentPreviousDecision = opponentDecision;

            _logger.LogInformation($"{player.GetType().Name} vs. {opponent.GetType().Name} {score.PlayerScore}/{score.OpponentScore}");
        }
    }
}