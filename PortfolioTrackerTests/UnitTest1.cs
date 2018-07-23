using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PortfolioTrackerTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string.Empty.Should().BeEmpty();
        }
    }
}