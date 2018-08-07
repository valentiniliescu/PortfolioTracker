using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using PortfolioTracker.PAS;

namespace PortfolioTracker.Model
{
    public sealed class PortfolioWithValue
    {
        [NotNull] private readonly IPortfolioStore _portfolioStore;
        [NotNull] private readonly Func<IEnumerable<Symbol>, Task<IEnumerable<Quote>>> _quoteLoader;

        public PortfolioWithValue([NotNull] IPortfolioStore portfolioStore, [NotNull] Func<IEnumerable<Symbol>, Task<IEnumerable<Quote>>> quoteLoader)
        {
            _portfolioStore = portfolioStore;
            _quoteLoader = quoteLoader;
        }

        public decimal TotalValue { get; private set; }

        [CanBeNull]
        public Portfolio Portfolio { get; private set; }

        public void Load()
        {
            Portfolio = _portfolioStore.Load();
        }

        public void Save()
        {
            _portfolioStore.Save(Portfolio);
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