using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using JetBrains.Annotations;

namespace PortfolioTracker.Model
{
    public sealed class Portfolio
    {
        [NotNull] private readonly IImmutableDictionary<Symbol, Asset> _assetsMap;

        public Portfolio(params Asset[] assets)
        {
            // ReSharper disable AssignNullToNotNullAttribute
            _assetsMap = assets.Aggregate<Asset, IImmutableDictionary<Symbol, Asset>>(ImmutableDictionary<Symbol, Asset>.Empty, AddAssetToMap);
            // ReSharper restore AssignNullToNotNullAttribute
        }

        private Portfolio([NotNull] IImmutableDictionary<Symbol, Asset> assetsMap)
        {
            _assetsMap = assetsMap;
        }

        [NotNull]
        [ItemNotNull]
        public IEnumerable<Asset> Assets => _assetsMap.Values;

        public bool HasAssets => _assetsMap.Count > 0;

        [Pure]
        [NotNull]
        // ReSharper disable once AssignNullToNotNullAttribute
        // ReSharper disable once PossibleNullReferenceException
        public Portfolio Clone() => new Portfolio(_assetsMap);

        [Pure]
        [NotNull]
        public Portfolio AddAsset([NotNull] Asset newAsset) => new Portfolio(AddAssetToMap(_assetsMap, newAsset));

        [Pure]
        [NotNull]
        private static IImmutableDictionary<Symbol, Asset> AddAssetToMap([NotNull] IImmutableDictionary<Symbol, Asset> assets, [NotNull] Asset newAsset)
        {
            if (assets.TryGetValue(newAsset.Symbol, out Asset existingAsset))
            {
                decimal amount = newAsset.Amount + existingAsset.Amount;
                if (amount < 0)
                {
                    throw new InvalidOperationException("Cannot add an asset that will result in negative amount in portfolio");
                }

                if (amount == 0)
                {
                    // ReSharper disable once AssignNullToNotNullAttribute
                    return assets.Remove(newAsset.Symbol);
                }

                // ReSharper disable once AssignNullToNotNullAttribute
                return assets.SetItem(newAsset.Symbol, new Asset(newAsset.Symbol, amount));
            }

            if (newAsset.Amount < 0)
            {
                throw new InvalidOperationException("Cannot add an asset that will result in negative amount in portfolio");
            }

            if (newAsset.Amount == 0)
            {
                return assets;
            }

            // ReSharper disable once AssignNullToNotNullAttribute
            return assets.SetItem(newAsset.Symbol, newAsset);
        }
    }
}