using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PortfolioTracker.Model;

namespace PortfolioTrackerTests.Model
{
    [TestClass]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public class PortfolioTests
    {
        [TestMethod]
        public void Constructor_should_add_assets()
        {
            var portfolio = new Portfolio(new Asset(new Symbol("MSFT"), 10), new Asset(new Symbol("AAPL"), 10));

            portfolio.Assets.Should().BeEquivalentTo(new Asset(new Symbol("MSFT"), 10), new Asset(new Symbol("AAPL"), 10));
        }

        [TestMethod]
        public void Assets_with_same_symbol_should_be_merged()
        {
            var portfolio = new Portfolio();

            portfolio = portfolio
                .AddAsset(new Asset(new Symbol("MSFT"), 10))
                .AddAsset(new Asset(new Symbol("AAPL"), 10))
                .AddAsset(new Asset(new Symbol("MSFT"), 20));

            portfolio.Assets.Should().BeEquivalentTo(new Asset(new Symbol("MSFT"), 30), new Asset(new Symbol("AAPL"), 10));
        }

        [TestMethod]
        public void Merged_assets_are_not_allowed_to_have_negative_amount()
        {
            var portfolio = new Portfolio();

            Action action = () => portfolio
                .AddAsset(new Asset(new Symbol("MSFT"), 10))
                .AddAsset(new Asset(new Symbol("AAPL"), 10))
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                .AddAsset(new Asset(new Symbol("MSFT"), -20));

            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void New_assets_are_not_allowed_to_have_negative_amount()
        {
            var portfolio = new Portfolio();

            Action action = () => portfolio
                .AddAsset(new Asset(new Symbol("MSFT"), 10))
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                .AddAsset(new Asset(new Symbol("MSFT"), -20));

            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void Adding_asset_with_negative_amount_is_allowed_if_the_merged_asset_has_positive_amount()
        {
            var portfolio = new Portfolio();

            portfolio = portfolio
                .AddAsset(new Asset(new Symbol("MSFT"), 20))
                .AddAsset(new Asset(new Symbol("AAPL"), 10))
                .AddAsset(new Asset(new Symbol("MSFT"), -10));

            portfolio.Assets.Should().BeEquivalentTo(new Asset(new Symbol("MSFT"), 10), new Asset(new Symbol("AAPL"), 10));
        }

        [TestMethod]
        public void Merged_assets_with_zero_amount_should_be_removed()
        {
            var portfolio = new Portfolio();

            portfolio = portfolio
                .AddAsset(new Asset(new Symbol("MSFT"), 20))
                .AddAsset(new Asset(new Symbol("AAPL"), 10))
                .AddAsset(new Asset(new Symbol("MSFT"), -20));

            portfolio.Assets.Should().BeEquivalentTo(new Asset(new Symbol("AAPL"), 10));
        }

        [TestMethod]
        public void Adding_new_asset_with_zero_amount_should_be_merged()
        {
            var portfolio = new Portfolio();

            portfolio = portfolio
                .AddAsset(new Asset(new Symbol("MSFT"), 20))
                .AddAsset(new Asset(new Symbol("AAPL"), 10))
                .AddAsset(new Asset(new Symbol("MSFT"), 0));

            portfolio.Assets.Should().BeEquivalentTo(new Asset(new Symbol("MSFT"), 20), new Asset(new Symbol("AAPL"), 10));
        }

        [TestMethod]
        public void Adding_new_asset_with_zero_amount_should_do_nothing()
        {
            var portfolio = new Portfolio();

            portfolio = portfolio
                .AddAsset(new Asset(new Symbol("AAPL"), 10))
                .AddAsset(new Asset(new Symbol("MSFT"), 0));

            portfolio.Assets.Should().BeEquivalentTo(new Asset(new Symbol("AAPL"), 10));
        }
    }
}