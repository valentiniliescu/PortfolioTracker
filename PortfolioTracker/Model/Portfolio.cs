using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using JetBrains.Annotations;

namespace PortfolioTracker.Model
{
    public sealed class Portfolio
    {
        [NotNull] private IImmutableDictionary<Symbol, Asset> _assets;

        public Portfolio()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            _assets = ImmutableDictionary<Symbol,Asset>.Empty;
   
        }

        private Portfolio([NotNull] IImmutableDictionary<Symbol, Asset> assets)
        {
            _assets = assets;
        }

        [NotNull]
        [ItemNotNull]
        public IEnumerable<Asset> Assets => _assets.Values;

        public bool HasAssets => _assets.Count > 0;

        [Pure]
        [NotNull]
        // ReSharper disable once AssignNullToNotNullAttribute
        // ReSharper disable once PossibleNullReferenceException
        public Portfolio Clone() => new Portfolio(_assets);

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
                    // ReSharper disable once AssignNullToNotNullAttribute
                    _assets = _assets.Remove(newAsset.Symbol);
                }
                else
                {
                    // ReSharper disable once AssignNullToNotNullAttribute
                    _assets = _assets.SetItem(newAsset.Symbol, new Asset(newAsset.Symbol, amount));
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
                    // ReSharper disable once AssignNullToNotNullAttribute
                    _assets = _assets.SetItem(newAsset.Symbol, newAsset);
                }
            }
        }
    }
}