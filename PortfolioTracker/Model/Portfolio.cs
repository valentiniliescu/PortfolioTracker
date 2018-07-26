using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace PortfolioTracker.Model
{
    public sealed class Portfolio
    {
        [NotNull] private readonly Dictionary<string, Asset> _assets;

        public Portfolio([NotNull] [ItemNotNull] IEnumerable<Asset> assets)
        {
            _assets = assets.ToDictionary(asset => asset.Symbol);
        }

        [NotNull]
        [ItemNotNull]
        public IEnumerable<Asset> Assets => _assets.Values;

        public bool HasAssets => _assets.Count > 0;

        public void AddAsset([NotNull] Asset newAsset)
        {
            if (_assets.TryGetValue(newAsset.Symbol, out Asset existingAsset))
            {
                decimal amount = newAsset.Amount + existingAsset.Amount;
                if (amount < 0)
                {
                    throw new ArgumentException(nameof(newAsset));
                }

                if (amount == 0)
                {
                    _assets.Remove(newAsset.Symbol);
                }
                else
                {
                    _assets[newAsset.Symbol] = new Asset(newAsset.Symbol, amount);
                }
            }
            else
            {
                if (newAsset.Amount < 0)
                {
                    throw new ArgumentException(nameof(newAsset));
                }

                if (newAsset.Amount > 0)
                {
                    _assets[newAsset.Symbol] = newAsset;
                }
            }
        }
    }
}