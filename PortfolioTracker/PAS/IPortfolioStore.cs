using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using PortfolioTracker.Model;

namespace PortfolioTracker.PAS
{
    public interface IPortfolioStore
    {
        [NotNull]
        Task Load();
        event EventHandler<Portfolio> PortfolioLoaded;
        event EventHandler<PortfolioStoreLoadException> PortfolioErrorOnLoad;

        [NotNull]
        Task Save([CanBeNull] Portfolio portfolio);
        event EventHandler PortfolioSaved;
        event EventHandler<PortfolioStoreSaveException> PortfolioErrorOnSave;
    }
}