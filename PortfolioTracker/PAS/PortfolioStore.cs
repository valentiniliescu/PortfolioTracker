using JetBrains.Annotations;
using PortfolioTracker.Model;

namespace PortfolioTracker.PAS
{
    public sealed class PortfolioStore
    {
        [CanBeNull] private Portfolio _portfolio;

        [NotNull]
        [MustUseReturnValue]
        public Portfolio Load()
        {
            if (_portfolio == null)
            {
                _portfolio = new Portfolio();
            }

            return _portfolio.Clone();
        }

        public void Save([CanBeNull] Portfolio portfolio)
        {
            _portfolio = portfolio?.Clone();
        }
    }
}