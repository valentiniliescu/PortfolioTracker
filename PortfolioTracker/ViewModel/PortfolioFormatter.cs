using JetBrains.Annotations;
using PortfolioTracker.Model;

namespace PortfolioTracker.ViewModel
{
    public static class PortfolioFormatter
    {
        [Pure]
        [CanBeNull]
        public static string Format([CanBeNull] Portfolio portfolio)
        {
            if (portfolio == null)
            {
                return null;
            }

            if (portfolio.HasAssets)

            {
                return $"You have {string.Join(", ", portfolio.Assets)} shares";
            }

            return "You have no assets";
        }
    }
}