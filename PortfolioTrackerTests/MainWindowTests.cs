using System.Windows.Controls;
using System.Windows.Data;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PortfolioTracker.PAS;
using PortfolioTracker.View;
using PortfolioTracker.ViewModel;

namespace PortfolioTrackerTests
{
    [TestClass]
    public class MainWindowTests
    {
        [TestMethod]
        public void Data_context_should_be_the_view_model()
        {
            var portfolioStore = new PortfolioStore();
            var viewModel = new MainViewModel(portfolioStore);

            var window = new MainWindow(viewModel);

            // ReSharper disable once PossibleNullReferenceException
            window.DataContext.Should().Be(viewModel);
        }

        [TestMethod]
        public void Main_text_block_should_be_bound_to_portfolio_description_of_view_model()
        {
            var portfolioStore = new PortfolioStore();
            var viewModel = new MainViewModel(portfolioStore);

            var window = new MainWindow(viewModel);

            // ReSharper disable AssignNullToNotNullAttribute
            Binding textBlockBinding = BindingOperations.GetBinding(window.MainTextBlock, TextBlock.TextProperty);
            // ReSharper restore AssignNullToNotNullAttribute

            // ReSharper disable PossibleNullReferenceException
            textBlockBinding.Should().NotBeNull();
            textBlockBinding.Path.Path.Should().Be(nameof(MainViewModel.PortfolioDescription));
            // ReSharper restore PossibleNullReferenceException
        }

        // TODO: test that events are binded to ViewModel methods
    }
}