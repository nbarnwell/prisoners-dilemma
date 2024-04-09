using Microsoft.Extensions.Logging;
using PrisonersDilemma.Runner;

namespace PrisonersDilemma.Tests
{
    public class Tests
    {
        [Test]
        public async Task GameRunner_runs_games()
        {
            IStrategyFactory strategyFactory = new TestStrategyFactory();

            var sut = new GameRunner(strategyFactory, new Logger<GameRunner>(new LoggerFactory()));

            var scoreboard = await sut.Run();

            Assert.AreEqual(1200, scoreboard.GameCount);
        }
    }
}