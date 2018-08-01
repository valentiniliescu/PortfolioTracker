using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PortfolioTracker.Model;
using PortfolioTracker.PAS;

namespace PortfolioTrackerTests
{
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public abstract class PortfolioStoreTests
    {
        protected abstract IPortfolioStore CreatePortfolioStore();

        [TestMethod]
        public void Loading_twice_should_return_different_portfolios()
        {
            IPortfolioStore portfolioStore = CreatePortfolioStore();

            Portfolio loadedPortfolio1 = portfolioStore.Load();
            Portfolio loadedPortfolio2 = portfolioStore.Load();

            loadedPortfolio1.Should().NotBeSameAs(loadedPortfolio2);

            portfolioStore.Save(null);
        }

        [TestMethod]
        public void Loading_after_saving_should_return_portfolio_with_the_same_assets()
        {
            IPortfolioStore portfolioStore = CreatePortfolioStore();
            var savedPortfolio = new Portfolio();
            savedPortfolio.AddAsset(new Asset(new Symbol("MSFT"), 100));
            portfolioStore.Save(savedPortfolio);

            Portfolio loadedPortfolio = portfolioStore.Load();

            loadedPortfolio.Assets.Should().BeEquivalentTo(savedPortfolio.Assets);

            portfolioStore.Save(null);
        }

        [TestMethod]
        public void Loading_without_saving_should_return_portfolios_with_the_same_assets()
        {
            IPortfolioStore portfolioStore = CreatePortfolioStore();
            var savedPortfolio = new Portfolio();
            savedPortfolio.AddAsset(new Asset(new Symbol("MSFT"), 100));
            portfolioStore.Save(savedPortfolio);

            Portfolio loadedPortfolio1 = portfolioStore.Load();
            Portfolio loadedPortfolio2 = portfolioStore.Load();

            loadedPortfolio1.Assets.Should().BeEquivalentTo(loadedPortfolio2.Assets);

            portfolioStore.Save(null);
        }

        [TestMethod]
        public void Loading_with_saving_in_between_should_return_portfolios_with_different_assets()
        {
            IPortfolioStore portfolioStore = CreatePortfolioStore();
            var savePortfolio = new Portfolio();
            savePortfolio.AddAsset(new Asset(new Symbol("MSFT"), 100));
            portfolioStore.Save(savePortfolio);

            Portfolio loadedPortfolio1 = portfolioStore.Load();

            savePortfolio.AddAsset(new Asset(new Symbol("AAPL"), 100));
            portfolioStore.Save(savePortfolio);

            Portfolio loadedPortfolio2 = portfolioStore.Load();

            loadedPortfolio1.Assets.Should().NotBeEquivalentTo(loadedPortfolio2.Assets);

            portfolioStore.Save(null);
        }
    }
}