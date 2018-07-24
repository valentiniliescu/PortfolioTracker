using System.Windows.Controls;
using System.Windows.Data;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PortfolioTracker;

namespace PortfolioTrackerTests
{
    [TestClass]
    public class MainWindowTests
    {
        [TestMethod]
        public void Data_context_should_be_the_view_model()
        {
            var assets = new Asset[0];
            var portfolio = new Portfolio(assets);
            var viewModel = new ViewModel(portfolio);

            var window = new MainWindow(viewModel);

            // ReSharper disable once PossibleNullReferenceException
            window.DataContext.Should().Be(viewModel);
        }

        [TestMethod]
        public void Main_text_block_should_be_bound_to_portfolio_description_of_view_model()
        {
            var assets = new Asset[0];
            var portfolio = new Portfolio(assets);
            var viewModel = new ViewModel(portfolio);

            var window = new MainWindow(viewModel);

            // ReSharper disable AssignNullToNotNullAttribute
            Binding textBlockBinding = BindingOperations.GetBinding(window.MainTextBlock, TextBlock.TextProperty);
            // ReSharper restore AssignNullToNotNullAttribute

            // ReSharper disable PossibleNullReferenceException
            textBlockBinding.Should().NotBeNull();
            textBlockBinding.Path.Path.Should().Be(nameof(ViewModel.PortfolioDescription));
            // ReSharper restore PossibleNullReferenceException
        }
    }
}