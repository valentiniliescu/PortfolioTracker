using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PortfolioTracker.Helpers;
using PortfolioTracker.View;
using PortfolioTracker.ViewModel;

namespace PortfolioTrackerTests.View
{
    [TestClass]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    public class MainWindowTests
    {
        [TestMethod]
        public void Main_text_block_should_be_bound_to_portfolio_description_of_view_model()
        {
            var viewModel = new MainViewModel();

            var window = new MainWindow(viewModel);

            CheckBinding(window.MainTextBlock, TextBlock.TextProperty, viewModel, nameof(MainViewModel.PortfolioDescription));
        }

        [TestMethod]
        public void New_asset_symbol_text_box_should_be_bound_to_view_model()
        {
            var viewModel = new MainViewModel();

            var window = new MainWindow(viewModel);

            CheckBinding(window.NewAssetSymbolTextBox, TextBox.TextProperty, viewModel, nameof(MainViewModel.NewAssetSymbol));
        }

        [TestMethod]
        public void New_asset_amount_text_box_should_be_bound_to_view_model()
        {
            var viewModel = new MainViewModel();

            var window = new MainWindow(viewModel);

            CheckBinding(window.NewAssetAmountTextBox, TextBox.TextProperty, viewModel, nameof(MainViewModel.NewAssetAmount));
        }

        [TestMethod]
        public void Error_text_block_should_be_bound_to_error_message_of_view_model()
        {
            var viewModel = new MainViewModel();

            var window = new MainWindow(viewModel);

            CheckBinding(window.ErrorTextBlock, TextBlock.TextProperty, viewModel, nameof(MainViewModel.ErrorMessage));
        }

        [TestMethod]
        public void Value_text_block_should_be_bound_to_portfolio_value_description_of_view_model()
        {
            var viewModel = new MainViewModel();

            var window = new MainWindow(viewModel);

            CheckBinding(window.ValueTextBlock, TextBlock.TextProperty, viewModel, nameof(MainViewModel.PortfolioValueDescription));
        }

        // TODO: find a better way to test the event bindings
#if DEBUG
        [TestMethod]
        public void Event_bindings_should_be_set()
        {
            var viewModel = new MainViewModel();

            try
            {
                EventBindingExtension.EventBindingStore = new ConcurrentBag<Tuple<DependencyObject, string, object, string>>();

                var mainWindow = new MainWindow(viewModel);

                EventBindingExtension.EventBindingStore.Should().BeEquivalentTo(
                    new Tuple<DependencyObject, string, object, string>(mainWindow, nameof(MainWindow.Loaded), viewModel, nameof(MainViewModel.Load)),
                    new Tuple<DependencyObject, string, object, string>(mainWindow.AddAssetButton, nameof(Button.Click), viewModel, nameof(MainViewModel.AddAsset)),
                    new Tuple<DependencyObject, string, object, string>(mainWindow.CalculateButton, nameof(Button.Click), viewModel, nameof(MainViewModel.Calculate)),
                    new Tuple<DependencyObject, string, object, string>(mainWindow, nameof(Window.Closing), viewModel, nameof(MainViewModel.Save))
                );
            }
            finally
            {
                EventBindingExtension.EventBindingStore = null;
            }
        }
#endif

        private static void CheckBinding(DependencyObject targetElement, DependencyProperty targetDependencyProperty, object source, string sourcePropertyName)
        {
            BindingExpression bindingExpression = BindingOperations.GetBindingExpression(targetElement, targetDependencyProperty);

            bindingExpression.Should().NotBeNull();
            bindingExpression.ResolvedSource.Should().Be(source);
            bindingExpression.ParentBinding.Path.Path.Should().Be(sourcePropertyName);
        }
    }
}