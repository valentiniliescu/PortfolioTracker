using JetBrains.Annotations;
using PortfolioTracker.Model;

namespace PortfolioTracker.PAS
{
    public sealed class PortfolioStore
    {
        [CanBeNull] private Portfolio _portfolio;

        [NotNull]
        public Portfolio Load()
        {
            return _portfolio ?? (_portfolio = new Portfolio());
        }

        public void Save([NotNull] Portfolio portfolio)
        {
            _portfolio = portfolio.Clone();
        }
    }
}