using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using FluentAssertions.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PortfolioTracker.PAS;
using PortfolioTracker.ViewModel;

namespace PortfolioTrackerTests
{
    [TestClass]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
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

            viewModel.PortfolioDescription.Should().Be("You have 100 MSFT, 10 AAPL shares");
        }

        [TestMethod]
        public void Portfolio_description_when_it_has_no_assets()
        {
            var viewModel = new MainViewModel(new PortfolioStore());
            viewModel.Load();

            viewModel.PortfolioDescription.Should().Be("You have no assets");
        }

        [TestMethod]
        public void Loading_assets_should_change_the_portfolio_description_and_fire_property_changed_event()
        {
            var viewModel = new MainViewModel(new PortfolioStore());

            viewModel.PortfolioDescription.Should().BeNull();

            using (IMonitor<MainViewModel> viewModelMonitored = viewModel.Monitor())
            {
                viewModel.Load();

                viewModelMonitored.Should().RaisePropertyChangeFor(vm => vm.PortfolioDescription);

                viewModel.PortfolioDescription.Should().Be("You have no assets");
            }
        }

        [TestMethod]
        public void Adding_an_asset_should_fire_property_changed_event()
        {
            var viewModel = new MainViewModel(new PortfolioStore());
            viewModel.Load();

            using (IMonitor<MainViewModel> viewModelMonitored = viewModel.Monitor())
            {
                viewModel.NewAssetSymbol = "MSFT";
                viewModel.NewAssetAmount = 100;
                viewModel.AddAsset();

                viewModelMonitored.Should().RaisePropertyChangeFor(vm => vm.PortfolioDescription);
            }
        }

        [TestMethod]
        public void Saving_should_save_the_assets_till_next_load()
        {
            var portfolioStore = new PortfolioStore();
            var viewModel = new MainViewModel(portfolioStore);
            viewModel.Load();

            viewModel.NewAssetSymbol = "MSFT";
            viewModel.NewAssetAmount = 100;
            viewModel.AddAsset();

            viewModel.Save();

            viewModel.NewAssetSymbol = "AAPL";
            viewModel.NewAssetAmount = 10;
            viewModel.AddAsset();

            viewModel.Load();

            viewModel.PortfolioDescription.Should().Be("You have 100 MSFT shares");
        }
    }
}