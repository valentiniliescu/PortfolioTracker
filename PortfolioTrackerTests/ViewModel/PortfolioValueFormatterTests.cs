using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PortfolioTracker.ViewModel;

namespace PortfolioTrackerTests.ViewModel
{
    [TestClass]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public class PortfolioValueFormatterTests
    {
        [TestMethod]
        public void Portfolio_value_format_when_it_has_zero_value()
        {
            const int value = 0;

            PortfolioValueFormatter.Format(value).Should().BeNull();
        }

        [TestMethod]
        public void Portfolio_value_format_when_it_has_value_more_than_zero()
        {
            const decimal value = 10.02m;

            PortfolioValueFormatter.Format(value).Should().Be("Total value $10.02");
        }
    }
}