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

        [Pure]
        public override string ToString() => $"{Amount} {Symbol}";
    }
}