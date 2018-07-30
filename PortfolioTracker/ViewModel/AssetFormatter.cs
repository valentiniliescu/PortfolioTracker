using JetBrains.Annotations;
using PortfolioTracker.Model;

namespace PortfolioTracker.ViewModel
{
    internal static class AssetFormatter
    {
        [Pure]
        [NotNull]
        [MustUseReturnValue]
        public static string Format([NotNull] Asset asset) => $"{asset.Amount} {asset.Symbol}";
    }
}