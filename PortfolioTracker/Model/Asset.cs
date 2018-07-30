using JetBrains.Annotations;

namespace PortfolioTracker.Model
{
    public sealed class Asset
    {
        public Asset([NotNull] string symbol, decimal amount)
        {
            Symbol = symbol;
            Amount = amount;
        }

        [NotNull]
        public string Symbol { get; }

        public decimal Amount { get; }
    }
}