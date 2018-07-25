using System.Collections.Generic;
using JetBrains.Annotations;
using PortfolioTracker.Model;

namespace PortfolioTracker.PAS
{
    public sealed class PortfolioStore
    {
        [NotNull] private Portfolio _portfolio = new Portfolio(new List<Asset>());

        [NotNull]
        public Portfolio Load()
        {
            return _portfolio;
        }

        public void Save([NotNull] Portfolio portfolio)
        {
            _portfolio = new Portfolio(portfolio.Assets);
        }
    }
}