﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using PortfolioTracker.PAS;

namespace PortfolioTrackerTests
{
    [TestClass]
    public class InMemoryPortfolioStoreTests : PortfolioStoreTests
    {
        protected override IPortfolioStore CreatePortfolioStore() => new InMemoryPortfolioStore();
    }
}