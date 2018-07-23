using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace PortfolioTracker
{
    public class PortfolioStore
    {
        private readonly List<Asset> _assets;

        public PortfolioStore([NotNull] IEnumerable<Asset> assets)
        {
            _assets = assets.ToList();
        }

        public IEnumerable<Asset> Assets => _assets;
    }
}