using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PortfolioTracker.Helpers;
using PortfolioTracker.PAS;
using PortfolioTracker.View;
using PortfolioTracker.ViewModel;

namespace PortfolioTrackerTests
{
    [TestClass]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    public class MainWindowTests
    {
        [TestMethod]
        public void Data_context_should_be_the_view_model()
        {
            var viewModel = new MainViewModel(new PortfolioStore());

            var window = new MainWindow(viewModel);

            window.DataContext.Should().Be(viewModel);
        }

        [TestMethod]
        public void Main_text_block_should_be_bound_to_portfolio_description_of_view_model()
        {
            var viewModel = new MainViewModel(new PortfolioStore());

            var window = new MainWindow(viewModel);

            CheckViewModelBinding(window.MainTextBlock, TextBlock.TextProperty, nameof(MainViewModel.PortfolioDescription));
        }

        [TestMethod]
        public void New_asset_symbol_text_box_should_be_bound_to_view_model()
        {
            var viewModel = new MainViewModel(new PortfolioStore());

            var window = new MainWindow(viewModel);

            CheckViewModelBinding(window.NewAssetSymbolTextBox, TextBox.TextProperty, nameof(MainViewModel.NewAssetSymbol));
        }

        [TestMethod]
        public void New_asset_amount_text_box_should_be_bound_to_view_model()
        {
            var viewModel = new MainViewModel(new PortfolioStore());

            var window = new MainWindow(viewModel);

            CheckViewModelBinding(window.NewAssetAmountTextBox, TextBox.TextProperty, nameof(MainViewModel.NewAssetAmount));
        }

        [TestMethod]
        public void Error_text_block_should_be_bound_to_error_message_of_view_model()
        {
            var viewModel = new MainViewModel(new PortfolioStore());

            var window = new MainWindow(viewModel);

            CheckViewModelBinding(window.ErrorTextBlock, TextBlock.TextProperty, nameof(MainViewModel.ErrorMessage));
        }

        [TestMethod]
        public void Event_bindings_should_be_set()
        {
            var viewModel = new MainViewModel(new PortfolioStore());

            EventBindingExtension.EventBindingStore = new ConcurrentBag<Tuple<string, string, string>>();

            // ReSharper disable ObjectCreationAsStatement
            new MainWindow(viewModel);
            // ReSharper restore ObjectCreationAsStatement

            EventBindingExtension.EventBindingStore.Should().BeEquivalentTo(
                new Tuple<string, string, string>(nameof(MainWindow.MainWindowWindow), nameof(Window.Loaded), nameof(MainViewModel.Load)),
                new Tuple<string, string, string>(nameof(MainWindow.AddAssetButton), nameof(Button.Click), nameof(MainViewModel.AddAsset)),
                new Tuple<string, string, string>(nameof(MainWindow.MainWindowWindow), nameof(Window.Closing), nameof(MainViewModel.Save))
            );
        }

        private static void CheckViewModelBinding(DependencyObject targetElement, DependencyProperty dependencyProperty, string propertyName)
        {
            Binding binding = BindingOperations.GetBinding(targetElement, dependencyProperty);

            binding.Should().NotBeNull();
            binding.Path.Path.Should().Be(propertyName);
        }
    }
}