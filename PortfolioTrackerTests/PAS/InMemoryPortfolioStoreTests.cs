using Microsoft.VisualStudio.TestTools.UnitTesting;
using PortfolioTracker.PAS;

namespace PortfolioTrackerTests.PAS
{
    [TestClass]
    public class InMemoryPortfolioStoreTests : PortfolioStoreTests
    {
        protected override IPortfolioStore CreatePortfolioStore() => new InMemoryPortfolioStore();

        protected override IPortfolioStore CreatePortfolioStoreWithSaveError() => new InMemoryPortfolioStore {ThrowOnSave = true};

        protected override IPortfolioStore CreatePortfolioStoreWithLoadError() => new InMemoryPortfolioStore {ThrowOnLoad = true};
    }
}