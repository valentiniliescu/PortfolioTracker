using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using PortfolioTracker.Model;

namespace PortfolioTracker.ViewModel
{
    public static class PortfolioFormatter
    {
        [Pure]
        [CanBeNull]
        [MustUseReturnValue]
        public static string Format([CanBeNull] Portfolio portfolio)
        {
            if (portfolio == null)
            {
                return null;
            }

            if (portfolio.HasAssets)
            {
                IEnumerable<string> assetsText = portfolio.Assets.Select(AssetFormatter.Format);
                return $"You have {string.Join(", ", assetsText)} shares";
            }

            return "You have no assets";
        }
    }
}