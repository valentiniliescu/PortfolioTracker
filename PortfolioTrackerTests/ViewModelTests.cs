using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PortfolioTracker;

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
            var viewModel = new ViewModel(portfolio);

            // ReSharper disable once PossibleNullReferenceException
            viewModel.PortfolioDescription.Should().Be("You have 100 MSFT shares");
        }

        [TestMethod]
        public void Portfolio_description_when_it_has_multiple_assets()
        {
            var assets = new[] {new Asset("MSFT", 100), new Asset("AAPL", 10)};
            var portfolio = new Portfolio(assets);
            var viewModel = new ViewModel(portfolio);

            // ReSharper disable once PossibleNullReferenceException
            viewModel.PortfolioDescription.Should().Be("You have 100 MSFT, 10 AAPL shares");
        }

        [TestMethod]
        public void Portfolio_description_when_it_has_no_assets()
        {
            var assets = new Asset[0];
            var portfolio = new Portfolio(assets);
            var viewModel = new ViewModel(portfolio);

            // ReSharper disable once PossibleNullReferenceException
            viewModel.PortfolioDescription.Should().Be("You have no assets");
        }
    }
}