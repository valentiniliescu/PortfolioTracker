using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using PortfolioTracker.PAS;

namespace PortfolioTracker.Model
{
    public static class PortfolioValueCalculator
    {
        public static async Task<decimal> Calculate([CanBeNull] Portfolio portfolio,[NotNull] QuoteLoaderDelegate quoteLoader)
        {
            decimal totalValue = 0;
            if (portfolio != null)
            {
                IEnumerable<Symbol> symbols = portfolio.Assets.Select(asset => asset.Symbol);

                // ReSharper disable once PossibleNullReferenceException
                IEnumerable<Quote> quotes = await quoteLoader(symbols);

                totalValue = portfolio.Assets
                    .Join(quotes, asset => asset.Symbol, quote => quote.Symbol, (asset, quote) => quote != null ? asset.Amount * quote.Price : 0)
                    .Sum();
            }

            return totalValue;
        }
    }
}