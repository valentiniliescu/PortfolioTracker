using System;
using JetBrains.Annotations;
using PortfolioTracker.Model;

namespace PortfolioTracker.PAS
{
    public interface IPortfolioStore
    {
        void Load();
        event EventHandler<Portfolio> PortfolioLoaded;
        event EventHandler<PortfolioStoreLoadException> PortfolioErrorOnLoad;

        void Save([CanBeNull] Portfolio portfolio);
        event EventHandler PortfolioSaved;
        event EventHandler<PortfolioStoreSaveException> PortfolioErrorOnSave;
    }
}