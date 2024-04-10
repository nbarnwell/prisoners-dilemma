using Microsoft.Extensions.Logging;
using PrisonersDilemma.Runner;

namespace PrisonersDilemma.Tests
{
    public class Tests
    {
        [Test]
        public async Task GameRunner_runs_games()
        {
            var strategyTypeProvider = new TestStrategyTypeProvider();
            var strategyFactory      = new TestStrategyFactory();

            var sut = new GameRunner(strategyTypeProvider, strategyFactory, new Logger<GameRunner>(new LoggerFactory()));

            var scoreboard = await sut.Run();

            Assert.AreEqual(200, scoreboard.GameCount);
        }
    }
}