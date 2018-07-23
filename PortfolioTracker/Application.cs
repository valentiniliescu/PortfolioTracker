using System.Collections.Generic;
using JetBrains.Annotations;

namespace PortfolioTracker
{
    public class Application
    {
        private readonly PortfolioStore _portfolioStore;

        public Application([NotNull] PortfolioStore portfolioStore)
        {
            _portfolioStore = portfolioStore;
        }

        [Pure]
        public string Render()
        {
            IEnumerable<Asset> assets = _portfolioStore.Assets;

            return $"You have {string.Join(", ", assets)} shares";
        }
    }
}