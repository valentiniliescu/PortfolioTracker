using System.Collections.Generic;
using PortfolioTracker.Model;

namespace PortfolioTracker.PAS
{
    public sealed class PortfolioStore
    {
        public Portfolio Load()
        {
            return new Portfolio(new List<Asset>());
        }
    }
}