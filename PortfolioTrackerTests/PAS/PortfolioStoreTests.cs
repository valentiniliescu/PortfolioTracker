using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PortfolioTracker.Model;
using PortfolioTracker.PAS;

namespace PortfolioTrackerTests.PAS
{
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public abstract class PortfolioStoreTests
    {
        protected abstract IPortfolioStore CreatePortfolioStore();
        protected abstract IPortfolioStore CreatePortfolioStoreWithSaveError();
        protected abstract IPortfolioStore CreatePortfolioStoreWithLoadError();

        [TestMethod]
        public async Task Loading_twice_should_return_different_portfolios()
        {
            IPortfolioStore portfolioStore = CreatePortfolioStore();

            Portfolio loadedPortfolio1 = await portfolioStore.Load();
            Portfolio loadedPortfolio2 = await portfolioStore.Load();

            loadedPortfolio1.Should().NotBeSameAs(loadedPortfolio2);

            await portfolioStore.Save(null);
        }

        [TestMethod]
        public async Task Loading_after_saving_should_return_portfolio_with_the_same_assets()
        {
            IPortfolioStore portfolioStore = CreatePortfolioStore();
            var savedPortfolio = new Portfolio();
            savedPortfolio.AddAsset(new Asset(new Symbol("MSFT"), 100));
            await portfolioStore.Save(savedPortfolio);

            Portfolio loadedPortfolio = await portfolioStore.Load();

            loadedPortfolio.Assets.Should().BeEquivalentTo(savedPortfolio.Assets);

            await portfolioStore.Save(null);
        }

        [TestMethod]
        public async Task Loading_without_saving_should_return_portfolios_with_the_same_assets()
        {
            IPortfolioStore portfolioStore = CreatePortfolioStore();
            var savedPortfolio = new Portfolio();
            savedPortfolio.AddAsset(new Asset(new Symbol("MSFT"), 100));
            await portfolioStore.Save(savedPortfolio);

            Portfolio loadedPortfolio1 = await portfolioStore.Load();
            Portfolio loadedPortfolio2 = await portfolioStore.Load();

            loadedPortfolio1.Assets.Should().BeEquivalentTo(loadedPortfolio2.Assets);

            await portfolioStore.Save(null);
        }

        [TestMethod]
        public async Task Loading_with_saving_in_between_should_return_portfolios_with_different_assets()
        {
            IPortfolioStore portfolioStore = CreatePortfolioStore();
            var savePortfolio = new Portfolio();
            savePortfolio.AddAsset(new Asset(new Symbol("MSFT"), 100));
            await portfolioStore.Save(savePortfolio);

            Portfolio loadedPortfolio1 = await portfolioStore.Load();

            savePortfolio.AddAsset(new Asset(new Symbol("AAPL"), 100));
            await portfolioStore.Save(savePortfolio);

            Portfolio loadedPortfolio2 = await portfolioStore.Load();

            loadedPortfolio1.Assets.Should().NotBeEquivalentTo(loadedPortfolio2.Assets);

            await portfolioStore.Save(null);
        }

        [TestMethod]
        public void Saving_error_should_throw_PortfolioStoreSaveException()
        {
            IPortfolioStore portfolioStore = CreatePortfolioStoreWithSaveError();

            Func<Task> action = async () => await portfolioStore.Save(new Portfolio());

            action.Should().Throw<PortfolioStoreSaveException>();
        }

        [TestMethod]
        public void Loading_error_should_throw_PortfolioStoreLoadException()
        {
            IPortfolioStore portfolioStore = CreatePortfolioStoreWithLoadError();

            Func<Task<Portfolio>> action = async () => await portfolioStore.Load();

            action.Should().Throw<PortfolioStoreLoadException>();
        }
    }
}