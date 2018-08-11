using System.Threading.Tasks;
using JetBrains.Annotations;
using PortfolioTracker.Model;

namespace PortfolioTracker.PAS
{
    public interface IPortfolioStore
    {
        [NotNull]
        [ItemNotNull]
        [MustUseReturnValue]
        Task<Portfolio> Load();

        [NotNull]
        Task Save([CanBeNull] Portfolio portfolio);
    }
}