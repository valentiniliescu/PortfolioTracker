using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using PortfolioTracker.Model;

namespace PortfolioTracker.PAS
{
    [NotNull]
    [ItemNotNull]
    public delegate Task<IEnumerable<Quote>> QuoteLoaderDelegate([NotNull] [ItemNotNull] IEnumerable<Symbol> symbols);
}