﻿using JetBrains.Annotations;
using PortfolioTracker.Model;

namespace PortfolioTracker.PAS
{
    public sealed class PortfolioStore : IPortfolioStore
    {
        [CanBeNull] private Portfolio _portfolio;

        [MustUseReturnValue]
        public Portfolio Load()
        {
            if (_portfolio == null)
            {
                _portfolio = new Portfolio();
            }

            return _portfolio.Clone();
        }

        public void Save(Portfolio portfolio)
        {
            _portfolio = portfolio?.Clone();
        }
    }
}