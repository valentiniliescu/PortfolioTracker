using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PortfolioTracker.Model;
using PortfolioTracker.PAS;

namespace PortfolioTrackerTests.Model
{
    [TestClass]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public class PortfolioValueCalculatorTests
    {
        [TestMethod]
        public async Task Calculate_should_return_the_portfolio_value()
        {
            IEnumerable<Quote> quotes = new[]
            {
                new Quote(new Symbol("MSFT"), 100),
                new Quote(new Symbol("AAPL"), 200)
            };
            // ReSharper disable once ConvertToLocalFunction
            QuoteLoaderDelegate quoteLoader = symbols => Task.FromResult(quotes);

            var portfolio = new Portfolio(new Asset(new Symbol("MSFT"), 10), new Asset(new Symbol("AAPL"), 10));

            (await PortfolioValueCalculator.Calculate(portfolio, quoteLoader)).Should().Be(3000);
        }

        [TestMethod]
        public async Task Calculate_should_skip_assets_that_have_no_corresponding_quotes()
        {
            IEnumerable<Quote> quotes = new[]
            {
                new Quote(new Symbol("MSFT"), 100)
            };
            // ReSharper disable once ConvertToLocalFunction
            QuoteLoaderDelegate quoteLoader = symbols => Task.FromResult(quotes);

            var portfolio = new Portfolio(new Asset(new Symbol("MSFT"), 10), new Asset(new Symbol("AAPL"), 10));

            (await PortfolioValueCalculator.Calculate(portfolio, quoteLoader)).Should().Be(1000);
        }

        [TestMethod]
        public async Task Calculate_should_be_OK_if_there_are_more_quotes_than_assets()
        {
            IEnumerable<Quote> quotes = new[]
            {
                new Quote(new Symbol("MSFT"), 100),
                new Quote(new Symbol("AAPL"), 200)
            };
            // ReSharper disable once ConvertToLocalFunction
            QuoteLoaderDelegate quoteLoader = symbols => Task.FromResult(quotes);

            var portfolio = new Portfolio(new Asset(new Symbol("MSFT"), 10));

            (await PortfolioValueCalculator.Calculate(portfolio, quoteLoader)).Should().Be(1000);
        }

        [TestMethod]
        public async Task Calculate_should_return_zero_if_portfolio_has_no_assets()
        {
            IEnumerable<Quote> quotes = new Quote[0];
            // ReSharper disable once ConvertToLocalFunction
            QuoteLoaderDelegate quoteLoader = symbols => Task.FromResult(quotes);

            var portfolio = new Portfolio();

            (await PortfolioValueCalculator.Calculate(portfolio, quoteLoader)).Should().Be(0);
        }

        [TestMethod]
        public async Task Calculate_should_return_zero_if_portfolio_is_null()
        {
            IEnumerable<Quote> quotes = new Quote[0];
            // ReSharper disable once ConvertToLocalFunction
            QuoteLoaderDelegate quoteLoader = symbols => Task.FromResult(quotes);

            (await PortfolioValueCalculator.Calculate(null, quoteLoader)).Should().Be(0);
        }
    }
}