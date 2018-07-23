namespace PortfolioTracker
{
    public class Application
    {
        private readonly PortfolioStore _portfolioStore;

        public Application(PortfolioStore portfolioStore)
        {
            _portfolioStore = portfolioStore;
        }

        public string Render()
        {
            return $"You have {_portfolioStore.Asset.Amount} {_portfolioStore.Asset.Symbol} shares";
        }
    }
}