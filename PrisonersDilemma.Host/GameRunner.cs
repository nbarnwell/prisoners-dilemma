using System.Reflection;

namespace PrisonersDilemma.Runner;

public class GameRunner
{
    private readonly IServiceProvider _services;

    public GameRunner(IServiceProvider services)
    {
        _services = services ?? throw new ArgumentNullException(nameof(services));
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
        Decision? playerDecision = null;
        Decision? opponentDecision = null;
        for (int i = 0; i < 200; i++)
        {
            playerDecision = await player.Decide(opponentDecision);
            opponentDecision = await opponent.Decide(playerDecision);
        }
    }
}