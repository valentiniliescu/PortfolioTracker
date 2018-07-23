namespace PortfolioTracker
{
    public class Asset
    {
        public Asset(string symbol, decimal amount)
        {
            Symbol = symbol;
            Amount = amount;
        }

        public string Symbol { get; }
        public decimal Amount { get; }
    }
}