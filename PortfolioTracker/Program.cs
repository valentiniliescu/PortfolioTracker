using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using PortfolioTracker.Model;
using PortfolioTracker.PAS;
using PortfolioTracker.View;
using PortfolioTracker.ViewModel;

namespace PortfolioTracker
{
    [ExcludeFromCodeCoverage]
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            var assetStore = new AssetStore();
            var portfolio = new Portfolio(assetStore);
            var viewModel = new MainViewModel(portfolio);

            var mainWindow = new MainWindow(viewModel);

            var application = new Application();
            application.Run(mainWindow);
        }
    }
}