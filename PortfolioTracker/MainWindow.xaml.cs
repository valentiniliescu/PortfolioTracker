using JetBrains.Annotations;

namespace PortfolioTracker
{
    public partial class MainWindow
    {
        public MainWindow([NotNull] ViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}