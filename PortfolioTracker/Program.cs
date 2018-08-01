using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
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
            var portfolioStore = new FilePortfolioStore();
            var viewModel = new MainViewModel(portfolioStore);

            var mainWindow = new MainWindow(viewModel);

            var application = new Application();
            application.Run(mainWindow);
        }
    }
}