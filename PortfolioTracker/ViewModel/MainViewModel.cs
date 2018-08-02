using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using PortfolioTracker.Model;
using PortfolioTracker.PAS;

namespace PortfolioTracker.ViewModel
{
    public sealed class MainViewModel : INotifyPropertyChanged
    {
        [NotNull] private readonly IPortfolioStore _portfolioStore;
        [CanBeNull] private string _errorMessage;

        public MainViewModel([NotNull] IPortfolioStore portfolioStore)
        {
            _portfolioStore = portfolioStore;
        }

        [CanBeNull]
        public Portfolio Portfolio { get; private set; }

        [CanBeNull]
        [ExcludeFromCodeCoverage]
        public string PortfolioDescription => PortfolioFormatter.Format(Portfolio);

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
            try
            {
                Portfolio = _portfolioStore.Load();
                OnPropertyChanged(nameof(PortfolioDescription));
            }
            catch (PortfolioStoreLoadException exception)
            {
                ErrorMessage = exception.Message;
            }
        }

        public void Save()
        {
            try
            {
                _portfolioStore.Save(Portfolio);
            }
            catch (PortfolioStoreSaveException exception)
            {
                ErrorMessage = exception.Message;
            }
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void AddAsset()
        {
            if (Portfolio != null && NewAssetSymbol != null)
            {
                try
                {
                    Portfolio.AddAsset(new Asset(new Symbol(NewAssetSymbol), NewAssetAmount));
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