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

            await runner?.Run()!;
        }
    }
}