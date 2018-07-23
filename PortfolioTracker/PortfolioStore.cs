using JetBrains.Annotations;

namespace PortfolioTracker
{
    public class PortfolioStore
    {
        public Asset Asset { get; private set; }

        public void AddAsset([NotNull] Asset asset)
        {
            Asset = asset;
        }
    }
}