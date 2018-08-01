using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PortfolioTracker.Model;
using PortfolioTracker.ViewModel;

namespace PortfolioTrackerTests
{
    [TestClass]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public class PortfolioFormatterTests
    {
        [TestMethod]
        public void Portfolio_format_when_it_has_a_single_asset()
        {
            var portfolio = new Portfolio();
            portfolio.AddAsset(new Asset(new Symbol("MSFT"), 100));

            PortfolioFormatter.Format(portfolio).Should().Be("You have 100 MSFT shares");
        }

        [TestMethod]
        public void Portfolio_format_when_it_has_multiple_assets()
        {
            var portfolio = new Portfolio();
            portfolio.AddAsset(new Asset(new Symbol("MSFT"), 100));
            portfolio.AddAsset(new Asset(new Symbol("AAPL"), 10));

            PortfolioFormatter.Format(portfolio).Should().Be("You have 100 MSFT, 10 AAPL shares");
        }

        [TestMethod]
        public void Portfolio_format_when_it_has_no_assets()
        {
            var portfolio = new Portfolio();

            PortfolioFormatter.Format(portfolio).Should().Be("You have no assets");
        }

        [TestMethod]
        public void Portfolio_format_when_there_is_no_portfolio()
        {
            PortfolioFormatter.Format(null).Should().BeNull();
        }
    }
}