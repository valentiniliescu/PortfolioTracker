﻿using System;
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
        [ProvidesContext]
        public Portfolio Portfolio => _portfolioWithValue.Portfolio;

        [ProvidesContext]
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

        public async Task Load()
        {
            try
            {
                await _portfolioWithValue.Load();
                OnPropertyChanged(nameof(PortfolioDescription));
                await _portfolioWithValue.Calculate();
                OnPropertyChanged(nameof(PortfolioValueDescription));
            }
            catch (PortfolioStoreLoadException exception)
            {
                ErrorMessage = exception.Message;
            }
        }

        public async Task Save()
        {
            try
            {
                await _portfolioWithValue.Save();
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

        public async Task AddAsset()
        {
            if (Portfolio != null && NewAssetSymbol != null)
            {
                try
                {
                    _portfolioWithValue.AddAsset(new Asset(new Symbol(NewAssetSymbol), NewAssetAmount));
                    OnPropertyChanged(nameof(PortfolioDescription));
                    await _portfolioWithValue.Calculate();
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