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
            var viewModel = new MainViewModel(new PortfolioWithValue(new FilePortfolioStore(), QuoteLoader.Load));

            var mainWindow = new MainWindow(viewModel);

            new Application().Run(mainWindow);
        }
    }
}