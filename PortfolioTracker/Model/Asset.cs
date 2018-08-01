using JetBrains.Annotations;

namespace PortfolioTracker.Model
{
    public sealed class Asset
    {
        public Asset([NotNull] Symbol symbol, decimal amount)
        {
            Symbol = symbol;
            Amount = amount;
        }

        [NotNull]
        public Symbol Symbol { get; }

        public decimal Amount { get; }
    }
}