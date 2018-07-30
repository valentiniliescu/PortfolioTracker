using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PortfolioTracker.Model;
using PortfolioTracker.PAS;

namespace PortfolioTrackerTests
{
    [TestClass]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public class PortfolioStoreTests
    {
        [TestMethod]
        public void Loading_twice_should_return_different_portfolios()
        {
            var portfolioStore = new PortfolioStore();

            Portfolio loadedPortfolio1 = portfolioStore.Load();
            Portfolio loadedPortfolio2 = portfolioStore.Load();

            loadedPortfolio1.Should().NotBeSameAs(loadedPortfolio2);
        }

        [TestMethod]
        public void Loading_after_saving_should_return_portfolio_with_the_same_assets()
        {
            var portfolioStore = new PortfolioStore();
            var savedPortfolio = new Portfolio();
            savedPortfolio.AddAsset(new Asset("MSFT", 100));
            portfolioStore.Save(savedPortfolio);

            Portfolio loadedPortfolio = portfolioStore.Load();

            loadedPortfolio.Assets.Should().BeEquivalentTo(savedPortfolio.Assets);
        }

        [TestMethod]
        public void Loading_without_saving_should_return_portfolios_with_the_same_assets()
        {
            var portfolioStore = new PortfolioStore();
            var savedPortfolio = new Portfolio();
            savedPortfolio.AddAsset(new Asset("MSFT", 100));
            portfolioStore.Save(savedPortfolio);

            Portfolio loadedPortfolio1 = portfolioStore.Load();
            Portfolio loadedPortfolio2 = portfolioStore.Load();

            loadedPortfolio1.Assets.Should().BeEquivalentTo(loadedPortfolio2.Assets);
        }

        [TestMethod]
        public void Loading_with_saving_in_between_should_return_portfolios_with_different_assets()
        {
            var portfolioStore = new PortfolioStore();
            var savePortfolio = new Portfolio();
            savePortfolio.AddAsset(new Asset("MSFT", 100));
            portfolioStore.Save(savePortfolio);

            Portfolio loadedPortfolio1 = portfolioStore.Load();

            savePortfolio.AddAsset(new Asset("AAPL", 100));
            portfolioStore.Save(savePortfolio);

            Portfolio loadedPortfolio2 = portfolioStore.Load();

            loadedPortfolio1.Assets.Should().NotBeEquivalentTo(loadedPortfolio2.Assets);
        }
    }
}