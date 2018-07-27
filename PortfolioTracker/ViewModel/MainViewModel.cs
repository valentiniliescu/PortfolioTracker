using System;
using System.ComponentModel;
using JetBrains.Annotations;
using PortfolioTracker.Model;
using PortfolioTracker.PAS;

namespace PortfolioTracker.ViewModel
{
    public sealed class MainViewModel : INotifyPropertyChanged
    {
        [NotNull] private readonly PortfolioStore _portfolioStore;
        [CanBeNull] private string _errorMessage;
        [CanBeNull] private Portfolio _portfolio;

        public MainViewModel([NotNull] PortfolioStore portfolioStore)
        {
            _portfolioStore = portfolioStore;
        }

        [CanBeNull]
        public string PortfolioDescription => PortfolioFormatter.Format(_portfolio);

        [CanBeNull]
        public string ErrorMessage
        {
            get => _errorMessage;
            private set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
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
                try
                {
                    _portfolio.AddAsset(new Asset(NewAssetSymbol, NewAssetAmount));
                    OnPropertyChanged(nameof(PortfolioDescription));
                }
                catch (Exception exception)
                {
                    ErrorMessage = exception.Message;
                }
            }
        }
    }
}