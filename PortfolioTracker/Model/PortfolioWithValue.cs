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
        [ProvidesContext]
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
            Portfolio = Portfolio?.AddAsset(newAsset);
        }

        public async Task Calculate()
        {
            TotalValue = await PortfolioValueCalculator.Calculate(Portfolio, _quoteLoader);
        }
    }
}