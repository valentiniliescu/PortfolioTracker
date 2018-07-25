using JetBrains.Annotations;
using PortfolioTracker.ViewModel;

namespace PortfolioTracker.View
{
    public partial class MainWindow
    {
        public MainWindow([NotNull] MainViewModel mainViewModel)
        {
            DataContext = mainViewModel;
            InitializeComponent();
        }
    }
}