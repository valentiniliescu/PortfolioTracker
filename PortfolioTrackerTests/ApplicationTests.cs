using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PortfolioTracker;

namespace PortfolioTrackerTests
{
    [TestClass]
    public class ApplicationTests
    {
        [TestMethod]
        public void Render_should_return_a_message_describing_the_portfolio_when_it_has_a_single_asset()
        {
            var assets = new[] {new Asset("MSFT", 100)};
            var portfolioStore = new PortfolioStore(assets);
            var application = new Application(portfolioStore);

            // ReSharper disable once PossibleNullReferenceException
            application.Render().Should().Be("You have 100 MSFT shares");
        }

        [TestMethod]
        public void Render_should_return_a_message_describing_the_portfolio_when_it_has_multiple_assets()
        {
            var assets = new[] {new Asset("MSFT", 100), new Asset("AAPL", 10)};
            var portfolioStore = new PortfolioStore(assets);
            var application = new Application(portfolioStore);

            // ReSharper disable once PossibleNullReferenceException
            application.Render().Should().Be("You have 100 MSFT, 10 AAPL shares");
        }

        [TestMethod]
        public void Render_should_return_a_message_describing_the_portfolio_when_it_has_no_assets()
        {
            var assets = new Asset[0];
            var portfolioStore = new PortfolioStore(assets);
            var application = new Application(portfolioStore);

            // ReSharper disable once PossibleNullReferenceException
            application.Render().Should().Be("You have no assets");
        }
    }
}