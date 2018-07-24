using System;
using System.Diagnostics.CodeAnalysis;

namespace PortfolioTracker
{
    [ExcludeFromCodeCoverage]
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            var assets = new Asset[0];
            var portfolioStore = new Portfolio(assets);
            var application = new Application(portfolioStore);

            var wpfApplication = new System.Windows.Application();
            wpfApplication.Run(new MainWindow(application));
        }
    }
}