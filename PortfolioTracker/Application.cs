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
            return $"You have {_portfolioStore.Asset.Amount} {_portfolioStore.Asset.Symbol} shares";
        }
    }
}