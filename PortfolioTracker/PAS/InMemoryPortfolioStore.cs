using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using PortfolioTracker.Model;

namespace PortfolioTracker.PAS
{
    public sealed class InMemoryPortfolioStore : IPortfolioStore
    {
        [CanBeNull] private Portfolio _portfolio;

        public bool ThrowOnLoad { private get; set; }

        public bool ThrowOnSave { private get; set; }

        public Task<Portfolio> Load()
        {
            if (ThrowOnLoad)
            {
                throw new PortfolioStoreLoadException("Error loading the store", new Exception());
            }

            if (_portfolio == null)
            {
                _portfolio = new Portfolio();
            }

            return Task.FromResult(_portfolio);
        }

        public Task Save(Portfolio portfolio)
        {
            if (ThrowOnSave)
            {
                throw new PortfolioStoreSaveException("Error saving the store", new Exception());
            }

            _portfolio = portfolio;

            return Task.CompletedTask;
        }
    }
}