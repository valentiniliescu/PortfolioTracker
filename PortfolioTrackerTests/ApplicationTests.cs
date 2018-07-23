using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PortfolioTracker;

namespace PortfolioTrackerTests
{
    [TestClass]
    public class ApplicationTests
    {
        [TestMethod]
        public void Render_should_return_the_hardcoded_value()
        {
            var application = new Application();
            application.Render().Should().Be("You have 100 MSFT shares");
        }
    }
}