using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PortfolioTracker.Model;

namespace PortfolioTrackerTests
{
    [TestClass]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public class PortfolioTests
    {
        [TestMethod]
        public void Assets_with_same_symbol_should_be_merged()
        {
            var portfolio = new Portfolio(new Asset[0]);

            portfolio.AddAsset(new Asset("MSFT", 10));
            portfolio.AddAsset(new Asset("AAPL", 10));
            portfolio.AddAsset(new Asset("MSFT", 20));

            var expectedAssets = new[] {new Asset("MSFT", 30), new Asset("AAPL", 10)};
            portfolio.Assets.Should().Equal(expectedAssets, AssetEqualityComparison);
        }

        [TestMethod]
        public void Merged_assets_are_not_allowed_to_have_negative_amount()
        {
            var portfolio = new Portfolio(new Asset[0]);

            portfolio.AddAsset(new Asset("MSFT", 10));
            portfolio.AddAsset(new Asset("AAPL", 10));

            Action action = () => portfolio.AddAsset(new Asset("MSFT", -20));

            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void New_assets_are_not_allowed_to_have_negative_amount()
        {
            var portfolio = new Portfolio(new Asset[0]);

            portfolio.AddAsset(new Asset("AAPL", 10));

            Action action = () => portfolio.AddAsset(new Asset("MSFT", -20));

            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void Adding_asset_with_negative_amount_is_allowed_if_the_merged_asset_has_positive_amount()
        {
            var portfolio = new Portfolio(new Asset[0]);

            portfolio.AddAsset(new Asset("MSFT", 20));
            portfolio.AddAsset(new Asset("AAPL", 10));
            portfolio.AddAsset(new Asset("MSFT", -10));

            var expectedAssets = new[] {new Asset("MSFT", 10), new Asset("AAPL", 10)};
            portfolio.Assets.Should().Equal(expectedAssets, AssetEqualityComparison);
        }

        [TestMethod]
        public void Merged_assets_with_zero_amount_should_be_removed()
        {
            var portfolio = new Portfolio(new Asset[0]);

            portfolio.AddAsset(new Asset("MSFT", 20));
            portfolio.AddAsset(new Asset("AAPL", 10));
            portfolio.AddAsset(new Asset("MSFT", -20));

            var expectedAssets = new[] {new Asset("AAPL", 10)};
            portfolio.Assets.Should().Equal(expectedAssets, AssetEqualityComparison);
        }

        [TestMethod]
        public void Adding_new_asset_with_zero_amount_should_be_merged()
        {
            var portfolio = new Portfolio(new Asset[0]);

            portfolio.AddAsset(new Asset("MSFT", 20));
            portfolio.AddAsset(new Asset("AAPL", 10));
            portfolio.AddAsset(new Asset("MSFT", 0));

            var expectedAssets = new[] {new Asset("MSFT", 20), new Asset("AAPL", 10)};
            portfolio.Assets.Should().Equal(expectedAssets, AssetEqualityComparison);
        }

        [TestMethod]
        public void Adding_new_asset_with_zero_amount_should_do_nothing()
        {
            var portfolio = new Portfolio(new Asset[0]);

            portfolio.AddAsset(new Asset("AAPL", 10));
            portfolio.AddAsset(new Asset("MSFT", 0));

            var expectedAssets = new[] {new Asset("AAPL", 10)};
            portfolio.Assets.Should().Equal(expectedAssets, AssetEqualityComparison);
        }

        private static bool AssetEqualityComparison([NotNull] Asset asset1, [NotNull] Asset asset2)
        {
            return asset1.Symbol == asset2.Symbol &&
                   asset1.Amount == asset2.Amount;
        }
    }
}