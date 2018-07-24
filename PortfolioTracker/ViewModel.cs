using System.Collections.Generic;
using JetBrains.Annotations;

namespace PortfolioTracker
{
    public class ViewModel
    {
        [NotNull] private readonly Portfolio _portfolio;

        public ViewModel([NotNull] Portfolio portfolio)
        {
            _portfolio = portfolio;
        }

        [NotNull]
        public string PortfolioDescription
        {
            get
            {
                IEnumerable<Asset> assets = _portfolio.Assets;

                return _portfolio.HasAssets
                    ? $"You have {string.Join(", ", assets)} shares"
                    : "You have no assets";
            }
        }
    }
}