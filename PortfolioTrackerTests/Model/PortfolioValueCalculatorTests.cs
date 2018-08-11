using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PortfolioTracker.Model;

namespace PortfolioTrackerTests.Model
{
    [TestClass]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public class PortfolioValueCalculatorTests
    {
        [TestMethod]
        public void Calculate_should_return_the_portfolio_value()
        {
            var quotes = new[]
            {
                new Quote(new Symbol("MSFT"), 100),
                new Quote(new Symbol("AAPL"), 200)
            };

            var portfolio = new Portfolio();
            portfolio.AddAsset(new Asset(new Symbol("MSFT"), 10));
            portfolio.AddAsset(new Asset(new Symbol("AAPL"), 10));

            PortfolioValueCalculator.Calculate(portfolio, quotes).Should().Be(3000);
        }

        [TestMethod]
        public void Calculate_should_skip_assets_that_have_no_corresponding_quotes()
        {
            var quotes = new[]
            {
                new Quote(new Symbol("MSFT"), 100)
            };

            var portfolio = new Portfolio();
            portfolio.AddAsset(new Asset(new Symbol("MSFT"), 10));
            portfolio.AddAsset(new Asset(new Symbol("AAPL"), 10));

            PortfolioValueCalculator.Calculate(portfolio, quotes).Should().Be(1000);
        }

        [TestMethod]
        public void Calculate_should_be_OK_if_there_are_more_quotes_than_assets()
        {
            var quotes = new[]
            {
                new Quote(new Symbol("MSFT"), 100),
                new Quote(new Symbol("AAPL"), 200)
            };

            var portfolio = new Portfolio();
            portfolio.AddAsset(new Asset(new Symbol("MSFT"), 10));

            PortfolioValueCalculator.Calculate(portfolio, quotes).Should().Be(1000);
        }

        [TestMethod]
        public void Calculate_should_return_zero_if_portfolio_has_no_assets()
        {
            var quotes = new Quote[0];

            var portfolio = new Portfolio();

            PortfolioValueCalculator.Calculate(portfolio, quotes).Should().Be(0);
        }

        [TestMethod]
        public void Calculate_should_return_zero_if_portfolio_is_null()
        {
            var quotes = new Quote[0];

            PortfolioValueCalculator.Calculate(null, quotes).Should().Be(0);
        }

        [TestMethod]
        public void Calculate_should_return_zero_if_quotes_is_null()
        {
            var portfolio = new Portfolio();

            PortfolioValueCalculator.Calculate(portfolio, null).Should().Be(0);
        }
    }
}