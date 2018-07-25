using FluentAssertions;
using FluentAssertions.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PortfolioTracker.Model;
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
            var assets = new[] {new Asset("MSFT", 100)};
            var portfolio = new Portfolio(assets);
            var viewModel = new MainViewModel(portfolio);

            // ReSharper disable once PossibleNullReferenceException
            viewModel.PortfolioDescription.Should().Be("You have 100 MSFT shares");
        }

        [TestMethod]
        public void Portfolio_description_when_it_has_multiple_assets()
        {
            var assets = new[] {new Asset("MSFT", 100), new Asset("AAPL", 10)};
            var portfolio = new Portfolio(assets);
            var viewModel = new MainViewModel(portfolio);

            // ReSharper disable once PossibleNullReferenceException
            viewModel.PortfolioDescription.Should().Be("You have 100 MSFT, 10 AAPL shares");
        }

        [TestMethod]
        public void Portfolio_description_when_it_has_no_assets()
        {
            var assets = new Asset[0];
            var portfolio = new Portfolio(assets);
            var viewModel = new MainViewModel(portfolio);

            // ReSharper disable once PossibleNullReferenceException
            viewModel.PortfolioDescription.Should().Be("You have no assets");
        }

        [TestMethod]
        public void Loading_assets_should_change_the_portfolio_description_and_fire_property_changed_event()
        {
            var assetStore = new AssetStore();
            var portfolio = new Portfolio(assetStore);
            var viewModel = new MainViewModel(portfolio);

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