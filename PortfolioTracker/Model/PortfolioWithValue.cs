using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using PortfolioTracker.PAS;

namespace PortfolioTracker.Model
{
    public sealed class PortfolioWithValue
    {
        // ReSharper disable once AssignNullToNotNullAttribute
        [NotNull] private static readonly QuoteLoaderDelegate DefaultQuoteLoader = symbols => Task.FromResult(symbols.Select(symbol => new Quote(symbol, 0)));
        [NotNull] private readonly IPortfolioStore _portfolioStore;
        [NotNull] private readonly QuoteLoaderDelegate _quoteLoader;

        public PortfolioWithValue([NotNull] IPortfolioStore portfolioStore, [NotNull] QuoteLoaderDelegate quoteLoader)
        {
            _portfolioStore = portfolioStore;
            _quoteLoader = quoteLoader;
        }

        public PortfolioWithValue() : this(new InMemoryPortfolioStore(), DefaultQuoteLoader)
        {
        }

        public PortfolioWithValue([NotNull] QuoteLoaderDelegate quoteLoader) : this(new InMemoryPortfolioStore(), quoteLoader)
        {
        }

        public PortfolioWithValue([NotNull] IPortfolioStore portfolioStore) : this(portfolioStore, DefaultQuoteLoader)
        {
        }

        public decimal TotalValue { get; private set; }

        [CanBeNull]
        public Portfolio Portfolio { get; private set; }

        public async Task Load()
        {
            Portfolio = await _portfolioStore.Load();
        }

        public async Task Save()
        {
            await _portfolioStore.Save(Portfolio);
        }

        public void AddAsset([NotNull] Asset newAsset)
        {
            Portfolio?.AddAsset(newAsset);
        }

        public async Task Calculate()
        {
            if (Portfolio != null)
            {
                IEnumerable<Symbol> symbols = Portfolio.Assets.Select(asset => asset.Symbol);

                // ReSharper disable once PossibleNullReferenceException
                IEnumerable<Quote> quotes = await _quoteLoader(symbols);

                TotalValue = Portfolio.Assets
                    .Join(quotes, asset => asset.Symbol, quote => quote.Symbol, (asset, quote) => quote != null ? asset.Amount * quote.Price : 0)
                    .Sum();
            }
            else
            {
                TotalValue = 0;
            }
        }
    }
}