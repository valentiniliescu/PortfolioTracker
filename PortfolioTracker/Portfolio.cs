using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace PortfolioTracker
{
    public sealed class Portfolio
    {
        [CanBeNull] [ItemNotNull] private List<Asset> _assets;

        [CanBeNull] private readonly AssetStore _assetStore;

        public Portfolio([NotNull] [ItemNotNull] IEnumerable<Asset> assets)
        {
            _assets = assets.ToList();
        }

        public Portfolio([NotNull] AssetStore assetStore)
        {
            _assetStore = assetStore;
        }

        [CanBeNull]
        [ItemNotNull]
        public IEnumerable<Asset> Assets => _assets;

        public bool HasAssets => _assets?.Count > 0;

        public bool AreAssetsLoaded => Assets != null;

        public void Load()
        {
            if (_assetStore != null)
            {
                _assets = _assetStore.Load();
            }
        }
    }
}