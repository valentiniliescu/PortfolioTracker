using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PortfolioTracker.Model;
using PortfolioTracker.PAS;

namespace PortfolioTrackerTests.Model
{
    [TestClass]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    public class PortfolioWithValueTests
    {
        [TestMethod]
        public void Portfolio_should_be_null_initially()
        {
            var portfolioWithValue = new PortfolioWithValue();

            portfolioWithValue.Portfolio.Should().BeNull();
        }

        [TestMethod]
        public async Task Portfolio_should_be_empty_after_loading()
        {
            var portfolioWithValue = new PortfolioWithValue();

            await portfolioWithValue.Load();

            portfolioWithValue.Portfolio.HasAssets.Should().BeFalse();
        }

        [TestMethod]
        public async Task Portfolio_should_have_assets_after_adding_assets()
        {
            var portfolioWithValue = new PortfolioWithValue();

            await portfolioWithValue.Load();
            await portfolioWithValue.AddAsset(new Asset(new Symbol("MSFT"), 10));
            await portfolioWithValue.AddAsset(new Asset(new Symbol("AAPL"), 10));

            portfolioWithValue.Portfolio.Assets.Should().BeEquivalentTo(new Asset(new Symbol("MSFT"), 10), new Asset(new Symbol("AAPL"), 10));
        }

        [TestMethod]
        public async Task Calculate_should_set_total_value()
        {
            // ReSharper disable once ConvertToLocalFunction
            QuoteLoaderDelegate quoteLoaderPrice100 = symbols => Task.FromResult(symbols.Select(symbol => new Quote(symbol, 100)));

            var portfolioWithValue = new PortfolioWithValue(quoteLoaderPrice100);

            await portfolioWithValue.Load();
            await portfolioWithValue.AddAsset(new Asset(new Symbol("MSFT"), 10));
            await portfolioWithValue.AddAsset(new Asset(new Symbol("AAPL"), 10));

            portfolioWithValue.TotalValue.Should().Be(2000);
        }
    }
}