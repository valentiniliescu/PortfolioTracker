using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PortfolioTracker.Model;
using PortfolioTracker.PAS;

namespace PortfolioTrackerTests.PAS
{
    [TestClass]
    [Ignore]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public class FilePortfolioStoreTests : PortfolioStoreTests
    {
        protected override IPortfolioStore CreatePortfolioStore() => new FilePortfolioStore(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));

        [TestMethod]
        public void Saving_error_should_throw_PortfolioStoreSaveException()
        {
            IPortfolioStore portfolioStore = new FilePortfolioStore("c:");

            Action action = () => portfolioStore.Save(new Portfolio());

            action.Should().Throw<PortfolioStoreSaveException>();
        }

        [TestMethod]
        [Ignore]
        public void Loading_error_should_throw_PortfolioStoreLoadException()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            IPortfolioStore portfolioStore = new FilePortfolioStore(null);

            // ReSharper disable once MustUseReturnValue
            Action action = () => portfolioStore.Load();

            action.Should().Throw<PortfolioStoreLoadException>();
        }
    }
}