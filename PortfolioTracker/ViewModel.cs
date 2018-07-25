using System.ComponentModel;
using JetBrains.Annotations;

namespace PortfolioTracker
{
    public sealed class ViewModel : INotifyPropertyChanged
    {
        [NotNull] private readonly Portfolio _portfolio;

        public ViewModel([NotNull] Portfolio portfolio)
        {
            _portfolio = portfolio;
        }

        [CanBeNull]
        public string PortfolioDescription
        {
            get
            {
                if (!_portfolio.AreAssetsLoaded)
                {
                    return null;
                }

                if (_portfolio.HasAssets)
                {
                    // ReSharper disable once AssignNullToNotNullAttribute
                    return $"You have {string.Join(", ", _portfolio.Assets)} shares";
                }

                return "You have no assets";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Load()
        {
            _portfolio.Load();
            OnPropertyChanged(nameof(PortfolioDescription));
        }

        public void Save()
        {
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}