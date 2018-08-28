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
            _assetsMap = assets.Aggregate<Asset, IImmutableDictionary<Symbol, Asset>>(ImmutableDictionary<Symbol, Asset>.Empty, MergeAssetWithMap);
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
        public Portfolio AddAsset([NotNull] Asset newAsset) => new Portfolio(MergeAssetWithMap(_assetsMap, newAsset));

        [Pure]
        [NotNull]
        private static IImmutableDictionary<Symbol, Asset> MergeAssetWithMap([NotNull] IImmutableDictionary<Symbol, Asset> assetsMap, [NotNull] Asset newAsset)
        {
            if (assetsMap.TryGetValue(newAsset.Symbol, out Asset existingAsset))
            {
                decimal amount = newAsset.Amount + existingAsset.Amount;
                if (amount < 0)
                {
                    throw new InvalidOperationException("Cannot add an asset that will result in negative amount in portfolio");
                }

                if (amount == 0)
                {
                    // ReSharper disable once AssignNullToNotNullAttribute
                    return assetsMap.Remove(newAsset.Symbol);
                }

                // ReSharper disable once AssignNullToNotNullAttribute
                return assetsMap.SetItem(newAsset.Symbol, new Asset(newAsset.Symbol, amount));
            }

            if (newAsset.Amount < 0)
            {
                throw new InvalidOperationException("Cannot add an asset that will result in negative amount in portfolio");
            }

            if (newAsset.Amount == 0)
            {
                return assetsMap;
            }

            // ReSharper disable once AssignNullToNotNullAttribute
            return assetsMap.SetItem(newAsset.Symbol, newAsset);
        }
    }
}