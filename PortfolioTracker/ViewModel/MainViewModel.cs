﻿using System.ComponentModel;
using JetBrains.Annotations;
using PortfolioTracker.Model;
using PortfolioTracker.PAS;

namespace PortfolioTracker.ViewModel
{
    public sealed class MainViewModel : INotifyPropertyChanged
    {
        [NotNull] private readonly PortfolioStore _portfolioStore;
        [CanBeNull] private Portfolio _portfolio;

        public MainViewModel([NotNull] PortfolioStore portfolioStore)
        {
            _portfolioStore = portfolioStore;
        }

        [CanBeNull]
        public string PortfolioDescription
        {
            get
            {
                if (_portfolio == null)
                {
                    return null;
                }

                if (_portfolio.HasAssets)

                {
                    return $"You have {string.Join(", ", _portfolio.Assets)} shares";
                }

                return "You have no assets";
            }
        }

        public string NewAssetSymbol { private get; set; }
        public decimal NewAssetAmount { private get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Load()
        {
            _portfolio = _portfolioStore.Load();
            OnPropertyChanged(nameof(PortfolioDescription));
        }

        public void Save()
        {
            if (_portfolio != null)
            {
                _portfolioStore.Save(_portfolio);
            }
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void AddAsset()
        {
            if (_portfolio != null && NewAssetSymbol != null)
            {
                _portfolio.AddAsset(new Asset(NewAssetSymbol, NewAssetAmount));
                OnPropertyChanged(nameof(PortfolioDescription));
            }
        }
    }
}