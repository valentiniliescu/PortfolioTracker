using JetBrains.Annotations;
using PortfolioTracker.Model;

namespace PortfolioTracker.PAS
{
    public sealed class PortfolioStore
    {
        [CanBeNull] private Portfolio _portfolio;

        [NotNull]
        public Portfolio Load() => _portfolio ?? (_portfolio = new Portfolio());

        public void Save([CanBeNull] Portfolio portfolio)
        {
            _portfolio = portfolio?.Clone();
        }
    }
}