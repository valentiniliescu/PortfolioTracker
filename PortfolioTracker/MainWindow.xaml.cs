using System.Diagnostics.CodeAnalysis;
using System.Windows;
using JetBrains.Annotations;

namespace PortfolioTracker
{
    [ExcludeFromCodeCoverage]
    public partial class MainWindow
    {
        [NotNull] private readonly Application _application;

        public MainWindow([NotNull] Application application)
        {
            _application = application;
            InitializeComponent();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (MainTextBlock != null)
            {
                MainTextBlock.Text = _application.Render();
            }
        }
    }
}