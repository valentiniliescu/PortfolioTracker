using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace PortfolioTracker
{
    [ExcludeFromCodeCoverage]
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            var assets = new Asset[0];
            var portfolio = new Portfolio(assets);
            var viewModel = new ViewModel(portfolio);

            var mainWindow = new MainWindow(viewModel);

            var application = new Application();
            application.Run(mainWindow);
        }
    }
}