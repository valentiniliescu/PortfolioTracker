namespace PortfolioTracker
{
    public class PortfolioStore
    {
        public string Symbol { get; private set; }
        public decimal Amount { get; private set; }

        public void AddAsset(string symbol, decimal amount)
        {
            Symbol = symbol;
            Amount = amount;
        }
    }
}