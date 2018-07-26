using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PortfolioTracker.Model;

namespace PortfolioTrackerTests
{
    [TestClass]
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
            // ReSharper disable PossibleNullReferenceException
            portfolio.Assets.Should().Equal(expectedAssets, AssetEqualityComparison);
            // ReSharper restore PossibleNullReferenceException
        }

        private static bool AssetEqualityComparison([NotNull] Asset asset1, [NotNull] Asset asset2)
        {
            return asset1.Symbol == asset2.Symbol &&
                   asset1.Amount == asset2.Amount;
        }
    }
}