using System.Collections.Generic;
using JetBrains.Annotations;

namespace PortfolioTracker
{
    public class Application
    {
        [NotNull] private readonly Portfolio _portfolio;

        public Application([NotNull] Portfolio portfolio)
        {
            _portfolio = portfolio;
        }

        [Pure]
        [NotNull]
        public string Render()
        {
            IEnumerable<Asset> assets = _portfolio.Assets;

            return _portfolio.HasAssets
                ? $"You have {string.Join(", ", assets)} shares"
                : "You have no assets";
        }
    }
}