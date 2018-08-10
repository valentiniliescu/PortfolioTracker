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
            var portfolioWithValue = new PortfolioWithValue(new InMemoryPortfolioStore(), symbols => Task.FromResult(symbols.Select(symbol => new Quote(symbol, 0))));

            portfolioWithValue.Portfolio.Should().BeNull();
        }

        [TestMethod]
        public void Portfolio_should_be_empty_after_loading()
        {
            var portfolioWithValue = new PortfolioWithValue(new InMemoryPortfolioStore(), symbols => Task.FromResult(symbols.Select(symbol => new Quote(symbol, 0))));

            portfolioWithValue.Load();

            portfolioWithValue.Portfolio.HasAssets.Should().BeFalse();
        }

        [TestMethod]
        public void Portfolio_should_have_assets_after_adding_assets()
        {
            var portfolioWithValue = new PortfolioWithValue(new InMemoryPortfolioStore(), symbols => Task.FromResult(symbols.Select(symbol => new Quote(symbol, 0))));

            portfolioWithValue.Load();
            portfolioWithValue.AddAsset(new Asset(new Symbol("MSFT"), 10));
            portfolioWithValue.AddAsset(new Asset(new Symbol("AAPL"), 10));

            portfolioWithValue.Portfolio.Assets.Should().BeEquivalentTo(new Asset(new Symbol("MSFT"), 10), new Asset(new Symbol("AAPL"), 10));
        }

        [TestMethod]
        public void Calculate_should_set_total_value()
        {
            var portfolioWithValue = new PortfolioWithValue(new InMemoryPortfolioStore(), symbols => Task.FromResult(symbols.Select(symbol => new Quote(symbol, 100))));

            portfolioWithValue.Load();
            portfolioWithValue.AddAsset(new Asset(new Symbol("MSFT"), 10));
            portfolioWithValue.AddAsset(new Asset(new Symbol("AAPL"), 10));
#pragma warning disable 4014
            portfolioWithValue.Calculate();
#pragma warning restore 4014

            portfolioWithValue.TotalValue.Should().Be(2000);
        }
    }
}