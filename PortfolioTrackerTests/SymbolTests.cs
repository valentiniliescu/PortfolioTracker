using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PortfolioTracker.Model;

namespace PortfolioTrackerTests
{
    [TestClass]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public class SymbolTests
    {
        [TestMethod]
        public void Text_should_be_case_insensitive()
        {
            var symbol1 = new Symbol("msft");
            var symbol2 = new Symbol("MSFT");

            symbol1.Should().Be(symbol2);
        }

        [TestMethod]
        public void Text_should_trim_whitespace()
        {
            var symbol1 = new Symbol("msft");
            var symbol2 = new Symbol("msft ");

            symbol1.Should().Be(symbol2);
        }
    }
}