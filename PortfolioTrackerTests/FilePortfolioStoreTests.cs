using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PortfolioTracker.PAS;

namespace PortfolioTrackerTests
{
    [TestClass]
    [Ignore]
    public class FilePortfolioStoreTests : PortfolioStoreTests
    {
        protected override IPortfolioStore CreatePortfolioStore() => new FilePortfolioStore(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));
    }
}