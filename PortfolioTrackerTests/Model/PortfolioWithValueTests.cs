using System;
using System.Collections.Generic;
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
            var portfolioStore = new InMemoryPortfolioStore();
            Func<IEnumerable<Symbol>, Task<IEnumerable<Quote>>> quoteLoader = symbols => Task.FromResult(symbols.Select(symbol => new Quote(symbol, 0)));
            var portfolioWithValue = new PortfolioWithValue(portfolioStore, quoteLoader);

            portfolioWithValue.Portfolio.Should().BeNull();
        }

        [TestMethod]
        public void Portfolio_should_be_empty_after_loading()
        {
            var portfolioStore = new InMemoryPortfolioStore();
            Func<IEnumerable<Symbol>, Task<IEnumerable<Quote>>> quoteLoader = symbols => Task.FromResult(symbols.Select(symbol => new Quote(symbol, 0)));
            var portfolioWithValue = new PortfolioWithValue(portfolioStore, quoteLoader);

            portfolioWithValue.Load();

            portfolioWithValue.Portfolio.HasAssets.Should().BeFalse();
        }

        [TestMethod]
        public void Portfolio_should_have_assets_after_adding_assets()
        {
            var portfolioStore = new InMemoryPortfolioStore();
            Func<IEnumerable<Symbol>, Task<IEnumerable<Quote>>> quoteLoader = symbols => Task.FromResult(symbols.Select(symbol => new Quote(symbol, 0)));
            var portfolioWithValue = new PortfolioWithValue(portfolioStore, quoteLoader);

            portfolioWithValue.Load();
            portfolioWithValue.AddAsset(new Asset(new Symbol("MSFT"), 10));
            portfolioWithValue.AddAsset(new Asset(new Symbol("AAPL"), 10));

            portfolioWithValue.Portfolio.Assets.Should().BeEquivalentTo(new Asset(new Symbol("MSFT"), 10), new Asset(new Symbol("AAPL"), 10));
        }

        [TestMethod]
        public void Calculate_should_set_total_value()
        {
            var portfolioStore = new InMemoryPortfolioStore();
            Func<IEnumerable<Symbol>, Task<IEnumerable<Quote>>> quoteLoader = symbols => Task.FromResult(symbols.Select(symbol => new Quote(symbol, 100)));
            var portfolioWithValue = new PortfolioWithValue(portfolioStore, quoteLoader);

            portfolioWithValue.Load();
            portfolioWithValue.AddAsset(new Asset(new Symbol("MSFT"), 10));
            portfolioWithValue.AddAsset(new Asset(new Symbol("AAPL"), 10));
            portfolioWithValue.Calculate();

            portfolioWithValue.TotalValue.Should().Be(2000);
        }
    }
}