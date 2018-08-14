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

            Portfolio loadedPortfolio1 = await GetLoadedPortfolio(portfolioStore);
            Portfolio loadedPortfolio2 = await GetLoadedPortfolio(portfolioStore);

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

            Portfolio loadedPortfolio = await GetLoadedPortfolio(portfolioStore);

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

            Portfolio loadedPortfolio1 = await GetLoadedPortfolio(portfolioStore);
            Portfolio loadedPortfolio2 = await GetLoadedPortfolio(portfolioStore);

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

            Portfolio loadedPortfolio1 = await GetLoadedPortfolio(portfolioStore);

            savePortfolio.AddAsset(new Asset(new Symbol("AAPL"), 100));
            await portfolioStore.Save(savePortfolio);

            Portfolio loadedPortfolio2 = await GetLoadedPortfolio(portfolioStore);

            loadedPortfolio1.Assets.Should().NotBeEquivalentTo(loadedPortfolio2.Assets);

            await portfolioStore.Save(null);
        }

        [TestMethod]
        public async Task Saving_error_should_throw_PortfolioStoreSaveException()
        {
            IPortfolioStore portfolioStore = CreatePortfolioStoreWithSaveError();

            using (var monitoredSubject = portfolioStore.Monitor())
            {
                await portfolioStore.Save(new Portfolio());
                monitoredSubject.Should().Raise(nameof(portfolioStore.PortfolioErrorOnSave));
            }
        }

        [TestMethod]
        public async Task Loading_error_should_throw_PortfolioStoreLoadException()
        {
            IPortfolioStore portfolioStore = CreatePortfolioStoreWithLoadError();
            using (var monitoredSubject = portfolioStore.Monitor())
            {
                await portfolioStore.Load();
                monitoredSubject.Should().Raise(nameof(portfolioStore.PortfolioErrorOnLoad));
            }
        }

        private static async Task<Portfolio> GetLoadedPortfolio(IPortfolioStore portfolioStore)
        {
            Portfolio loadedPortfolio = null;
            // ReSharper disable once ConvertToLocalFunction
            EventHandler<Portfolio> portfolioStoreOnPortfolioLoaded = (sender, portfolio) => { loadedPortfolio = portfolio; };
            portfolioStore.PortfolioLoaded += portfolioStoreOnPortfolioLoaded;

            await portfolioStore.Load();
            portfolioStore.PortfolioLoaded -= portfolioStoreOnPortfolioLoaded;
            return loadedPortfolio;
        }
    }
}