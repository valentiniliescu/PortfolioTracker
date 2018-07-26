﻿using System.Diagnostics.CodeAnalysis;
using System.Windows;
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

        private static void CheckViewModelBinding(DependencyObject targetElement, DependencyProperty dependencyProperty, string propertyName)
        {
            Binding binding = BindingOperations.GetBinding(targetElement, dependencyProperty);

            binding.Should().NotBeNull();
            binding.Path.Path.Should().Be(propertyName);
        }

        // TODO: test that events are binded to ViewModel methods
    }
}