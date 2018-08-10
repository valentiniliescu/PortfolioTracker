using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PortfolioTracker.Model;
using PortfolioTracker.PAS;

namespace PortfolioTrackerTests.PAS
{
    [TestClass]
    [Ignore]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public class QuoteLoaderTests
    {
        [TestMethod]
        public async Task Should_load_quotes()
        {
            var symbols = new[] {new Symbol("MSFT"), new Symbol("AAPL")};
            Quote[] quotes = (await QuoteLoader.Load(symbols)).ToArray();

            quotes.Should().HaveCount(2);
            quotes.First().Symbol.Should().Be(new Symbol("MSFT"));
            quotes.First().Price.Should().BeGreaterThan(0);
            quotes.Skip(1).First().Symbol.Should().Be(new Symbol("AAPL"));
            quotes.Skip(1).First().Price.Should().BeGreaterThan(0);
        }

        [TestMethod]
        public async Task Should_skip_invalid_symbols()
        {
            var symbols = new[] {new Symbol("MSFT"), new Symbol("XXX")};
            Quote[] quotes = (await QuoteLoader.Load(symbols)).ToArray();

            quotes.Should().HaveCount(1);
            quotes.First().Symbol.Should().Be(new Symbol("MSFT"));
            quotes.First().Price.Should().BeGreaterThan(0);
        }
    }
}