using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace PortfolioTracker
{
    [ExcludeFromCodeCoverage]
    public partial class MainWindow
    {
        public MainWindow([NotNull] ViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}