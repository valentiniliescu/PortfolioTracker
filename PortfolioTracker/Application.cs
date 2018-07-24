using System.Collections.Generic;
using JetBrains.Annotations;

namespace PortfolioTracker
{
    public class Application
    {
        [NotNull] private readonly PortfolioStore _portfolioStore;

        public Application([NotNull] PortfolioStore portfolioStore)
        {
            _portfolioStore = portfolioStore;
        }

        [Pure]
        [NotNull]
        public string Render()
        {
            IEnumerable<Asset> assets = _portfolioStore.Assets;

            return _portfolioStore.HasAssets
                ? $"You have {string.Join(", ", assets)} shares"
                : "You have no assets";
        }
    }
}