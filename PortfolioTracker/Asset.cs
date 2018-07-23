using JetBrains.Annotations;

namespace PortfolioTracker
{
    public class Asset
    {
        public Asset([NotNull] string symbol, decimal amount)
        {
            Symbol = symbol;
            Amount = amount;
        }

        public string Symbol { get; }
        public decimal Amount { get; }

        public override string ToString()
        {
            return $"{Amount} {Symbol}";
        }
    }
}