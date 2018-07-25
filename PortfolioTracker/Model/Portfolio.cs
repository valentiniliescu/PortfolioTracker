using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace PortfolioTracker.Model
{
    public sealed class Portfolio
    {
        [NotNull] [ItemNotNull] private readonly List<Asset> _assets;

        public Portfolio([NotNull] [ItemNotNull] IEnumerable<Asset> assets)
        {
            _assets = assets.ToList();
        }

        [NotNull]
        [ItemNotNull]
        public IEnumerable<Asset> Assets => _assets;

        public bool HasAssets => _assets.Count > 0;

        public void AddAsset(Asset asset)
        {
            _assets.Add(asset);
        }
    }
}