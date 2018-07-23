using System;
using System.Diagnostics.CodeAnalysis;

namespace PortfolioTracker
{
    [ExcludeFromCodeCoverage]
    internal static class Program
    {
        private static void Main()
        {
            var assets = new Asset[0];
            var portfolioStore = new PortfolioStore(assets);
            var application = new Application(portfolioStore);

            Console.WriteLine(application.Render());
        }
    }
}