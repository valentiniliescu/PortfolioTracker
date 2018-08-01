using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace PortfolioTracker.Model
{
    public sealed class Portfolio
    {
        [NotNull] private readonly Dictionary<Symbol, Asset> _assets;

        public Portfolio() : this(new Dictionary<Symbol, Asset>())
        {
        }

        private Portfolio([NotNull] Dictionary<Symbol, Asset> assets)
        {
            _assets = assets;
        }

        [NotNull]
        [ItemNotNull]
        public IEnumerable<Asset> Assets => _assets.Values;

        public bool HasAssets => _assets.Count > 0;

        [Pure]
        [NotNull]
        public Portfolio Clone() => new Portfolio(new Dictionary<Symbol, Asset>(_assets));

        public void AddAsset([NotNull] Asset newAsset)
        {
            if (_assets.TryGetValue(newAsset.Symbol, out Asset existingAsset))
            {
                decimal amount = newAsset.Amount + existingAsset.Amount;
                if (amount < 0)
                {
                    throw new InvalidOperationException("Cannot add an asset that will result in negative amount in portfolio");
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
                    throw new InvalidOperationException("Cannot add an asset that will result in negative amount in portfolio");
                }

                if (newAsset.Amount > 0)
                {
                    _assets[newAsset.Symbol] = newAsset;
                }
            }
        }
    }
}