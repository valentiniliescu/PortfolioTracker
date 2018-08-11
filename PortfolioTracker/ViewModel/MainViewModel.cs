using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using JetBrains.Annotations;
using PortfolioTracker.Model;
using PortfolioTracker.PAS;

namespace PortfolioTracker.ViewModel
{
    public sealed class MainViewModel : INotifyPropertyChanged
    {
        [NotNull] private readonly PortfolioWithValue _portfolioWithValue;
        [CanBeNull] private string _errorMessage;

        public MainViewModel([NotNull] PortfolioWithValue portfolioWithValue)
        {
            _portfolioWithValue = portfolioWithValue;
        }

        public MainViewModel() : this(new PortfolioWithValue())
        {
        }

        [CanBeNull]
        public Portfolio Portfolio => _portfolioWithValue.Portfolio;

        public decimal TotalValue => _portfolioWithValue.TotalValue;

        [CanBeNull]
        [ExcludeFromCodeCoverage]
        public string PortfolioDescription => PortfolioFormatter.Format(Portfolio);

        [CanBeNull]
        [ExcludeFromCodeCoverage]
        public string PortfolioValueDescription => PortfolioValueFormatter.Format(TotalValue);

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
                _portfolioWithValue.Load();
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
                _portfolioWithValue.Save();
            }
            catch (PortfolioStoreSaveException exception)
            {
                ErrorMessage = exception.Message;
            }
        }

        public async Task Calculate()
        {
            try
            {
                await _portfolioWithValue.Calculate();
                OnPropertyChanged(nameof(PortfolioValueDescription));
            }
            catch (Exception exception)
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
                    _portfolioWithValue.AddAsset(new Asset(new Symbol(NewAssetSymbol), NewAssetAmount));
                    OnPropertyChanged(nameof(PortfolioDescription));
                    OnPropertyChanged(nameof(PortfolioValueDescription));
                }
                catch (Exception exception)
                {
                    ErrorMessage = exception.Message;
                }
            }
        }
    }
}