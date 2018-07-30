using JetBrains.Annotations;
using PortfolioTracker.Model;

namespace PortfolioTracker.PAS
{
    public interface IPortfolioStore
    {
        [NotNull]
        [MustUseReturnValue]
        Portfolio Load();

        void Save([CanBeNull] Portfolio portfolio);
    }
}