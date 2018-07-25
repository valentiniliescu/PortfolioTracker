﻿using JetBrains.Annotations;

namespace PortfolioTracker
{
    public sealed class Asset
    {
        public Asset([NotNull] string symbol, decimal amount)
        {
            Symbol = symbol;
            Amount = amount;
        }

        private string Symbol { get; }
        private decimal Amount { get; }

        [Pure]
        public override string ToString() => $"{Amount} {Symbol}";
    }
}