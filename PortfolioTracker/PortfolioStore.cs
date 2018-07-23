namespace PortfolioTracker
{
    public class PortfolioStore
    {
        public Asset Asset { get; private set; }

        public void AddAsset(Asset asset)
        {
            Asset = asset;
        }
    }
}