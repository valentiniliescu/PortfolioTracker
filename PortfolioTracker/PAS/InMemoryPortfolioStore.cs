using System;
using JetBrains.Annotations;
using PortfolioTracker.Model;

namespace PortfolioTracker.PAS
{
    public sealed class InMemoryPortfolioStore : IPortfolioStore
    {
        [CanBeNull] private Portfolio _portfolio;

        public bool ThrowOnLoad { private get; set; }

        public bool ThrowOnSave { private get; set; }

        public event EventHandler<Portfolio> PortfolioLoaded;
        public event EventHandler<PortfolioStoreLoadException> PortfolioErrorOnLoad;

        public event EventHandler PortfolioSaved;
        public event EventHandler<PortfolioStoreSaveException> PortfolioErrorOnSave;

        public void Load()
        {
            if (ThrowOnLoad)
            {
                PortfolioErrorOnLoad?.Invoke(this, new PortfolioStoreLoadException("Error loading the store", new Exception()));
            }

            if (_portfolio == null)
            {
                _portfolio = new Portfolio();
            }

            PortfolioLoaded?.Invoke(this, _portfolio.Clone());
        }

        public void Save(Portfolio portfolio)
        {
            if (ThrowOnSave)
            {
                PortfolioErrorOnSave?.Invoke(this, new PortfolioStoreSaveException("Error saving the store", new Exception()));
            }

            _portfolio = portfolio?.Clone();

            PortfolioSaved?.Invoke(this, EventArgs.Empty);
        }
    }
}