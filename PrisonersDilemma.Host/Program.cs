using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PrisonersDilemma.Runner
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder();
            builder.Services
                   .AddLogging()
                   .AddStrategies()
                   .AddServices();

            var host = builder.Build();

            var runner = host.Services.GetService<GameRunner>();

            if (runner != null)
            {
                var scoreboard = await runner.Run();
                PrintScoreboard(scoreboard);
            }
            else
            {
                throw new InvalidOperationException("No game runner available");
            }
        }

        private static void PrintScoreboard(ScoreBoard scoreboard)
        {
            Console.WriteLine("");
            Console.WriteLine("Results:");

            foreach (var result in scoreboard.GetResults().OrderByDescending(x => x.Score))
            {
                Console.WriteLine($"{result.StrategyName}: {result.Score}");
            }
        }
    }
}