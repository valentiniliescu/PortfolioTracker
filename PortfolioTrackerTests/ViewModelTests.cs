using FluentAssertions;
using FluentAssertions.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PortfolioTracker.PAS;
using PortfolioTracker.ViewModel;

namespace PortfolioTrackerTests
{
    [TestClass]
    public class ViewModelTests
    {
        [TestMethod]
        public void Portfolio_description_when_it_has_a_single_asset()
        {
            var viewModel = new MainViewModel(new PortfolioStore());
            viewModel.Load();

            viewModel.NewAssetSymbol = "MSFT";
            viewModel.NewAssetAmount = 100;
            viewModel.AddAsset();

            // ReSharper disable once PossibleNullReferenceException
            viewModel.PortfolioDescription.Should().Be("You have 100 MSFT shares");
        }

        [TestMethod]
        public void Portfolio_description_when_it_has_multiple_assets()
        {
            var viewModel = new MainViewModel(new PortfolioStore());
            viewModel.Load();

            viewModel.NewAssetSymbol = "MSFT";
            viewModel.NewAssetAmount = 100;
            viewModel.AddAsset();

            viewModel.NewAssetSymbol = "AAPL";
            viewModel.NewAssetAmount = 10;
            viewModel.AddAsset();

            // ReSharper disable once PossibleNullReferenceException
            viewModel.PortfolioDescription.Should().Be("You have 100 MSFT, 10 AAPL shares");
        }

        [TestMethod]
        public void Portfolio_description_when_it_has_no_assets()
        {
            var viewModel = new MainViewModel(new PortfolioStore());
            viewModel.Load();

            // ReSharper disable once PossibleNullReferenceException
            viewModel.PortfolioDescription.Should().Be("You have no assets");
        }

        [TestMethod]
        public void Loading_assets_should_change_the_portfolio_description_and_fire_property_changed_event()
        {
            var viewModel = new MainViewModel(new PortfolioStore());

            // ReSharper disable once PossibleNullReferenceException
            viewModel.PortfolioDescription.Should().BeNull();

            using (IMonitor<MainViewModel> viewModelMonitored = viewModel.Monitor())
            {
                viewModel.Load();

                // ReSharper disable PossibleNullReferenceException
                viewModelMonitored.Should().RaisePropertyChangeFor(vm => vm.PortfolioDescription);
                // ReSharper restore PossibleNullReferenceException

                // ReSharper disable once PossibleNullReferenceException
                viewModel.PortfolioDescription.Should().Be("You have no assets");
            }
        }
    }
}