using System.Threading.Tasks;
using JetBrains.Annotations;
using PortfolioTracker.Model;

namespace PortfolioTracker.PAS
{
    public interface IPortfolioStore
    {
        [NotNull]
        [MustUseReturnValue]
        Task<Portfolio> Load();

        Task Save([CanBeNull] Portfolio portfolio);
    }
}