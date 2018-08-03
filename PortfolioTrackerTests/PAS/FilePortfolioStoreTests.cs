using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PortfolioTracker.PAS;

namespace PortfolioTrackerTests.PAS
{
    [TestClass]
    [Ignore]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    [DeploymentItem("Files/Invalid.txt")]
    public class FilePortfolioStoreTests : PortfolioStoreTests
    {
        protected override IPortfolioStore CreatePortfolioStore() => new FilePortfolioStore(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));

        protected override IPortfolioStore CreatePortfolioStoreWithSaveError() => new FilePortfolioStore("c:");

        protected override IPortfolioStore CreatePortfolioStoreWithLoadError() => new FilePortfolioStore("Invalid.txt");
    }
}