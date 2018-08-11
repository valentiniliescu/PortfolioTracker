using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace PortfolioTracker.Model
{
    public static class PortfolioValueCalculator
    {
        [Pure]
        [MustUseReturnValue]
        public static decimal Calculate([CanBeNull] Portfolio portfolio, [CanBeNull] [ItemNotNull] IEnumerable<Quote> quotes) =>
            portfolio != null && quotes != null
                ? portfolio.Assets
                    .Join(quotes, asset => asset.Symbol, quote => quote.Symbol, (asset, quote) => quote != null ? asset.Amount * quote.Price : 0)
                    .Sum()
                : 0;
    }
}