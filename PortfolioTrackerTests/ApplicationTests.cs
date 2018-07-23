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
            var portfolioStore = new PortfolioStore();
            var application = new Application(portfolioStore);
            portfolioStore.AddAsset(new Asset("MSFT", 100));

            application.Render().Should().Be("You have 100 MSFT shares");
        }
    }
}