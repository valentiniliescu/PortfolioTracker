using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using FluentAssertions;
using FluentAssertions.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PortfolioTracker.PAS;
using PortfolioTracker.ViewModel;
using PortfolioTrackerTests.Helpers;

namespace PortfolioTrackerTests
{
    [TestClass]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public class ViewModelTests
    {
        [TestMethod]
        public void Portfolio_description_should_call_formatter()
        {
            var viewModel = new MainViewModel(new PortfolioStore());

            LambdaExpression fromPropertyGetter = LambdaExpressionConverter.FromPropertyGetter(viewModel, nameof(viewModel.PortfolioDescription));

            CheckIsStaticMethodCall(fromPropertyGetter, typeof(PortfolioFormatter), nameof(PortfolioFormatter.Format));

            // TODO: check the method call argument is Portfolio
        }

        [TestMethod]
        public void Loading_assets_should_change_the_portfolio_description_and_fire_property_changed_event()
        {
            var viewModel = new MainViewModel(new PortfolioStore());

            viewModel.PortfolioDescription.Should().BeNull();

            using (IMonitor<MainViewModel> viewModelMonitored = viewModel.Monitor())
            {
                viewModel.Load();

                viewModelMonitored.Should().RaisePropertyChangeFor(vm => vm.PortfolioDescription);

                viewModel.PortfolioDescription.Should().Be("You have no assets");
            }
        }

        [TestMethod]
        public void Adding_an_asset_should_change_the_portfolio_description_and_fire_property_changed_event()
        {
            var viewModel = new MainViewModel(new PortfolioStore());
            viewModel.Load();

            using (IMonitor<MainViewModel> viewModelMonitored = viewModel.Monitor())
            {
                viewModel.NewAssetSymbol = "MSFT";
                viewModel.NewAssetAmount = 100;
                viewModel.AddAsset();

                viewModelMonitored.Should().RaisePropertyChangeFor(vm => vm.PortfolioDescription);
                viewModel.PortfolioDescription.Should().Be("You have 100 MSFT shares");
            }
        }

        [TestMethod]
        public void Saving_should_save_the_assets_till_next_load()
        {
            var portfolioStore = new PortfolioStore();
            var viewModel = new MainViewModel(portfolioStore);
            viewModel.Load();

            viewModel.NewAssetSymbol = "MSFT";
            viewModel.NewAssetAmount = 100;
            viewModel.AddAsset();

            viewModel.Save();

            viewModel.NewAssetSymbol = "AAPL";
            viewModel.NewAssetAmount = 10;
            viewModel.AddAsset();

            viewModel.Load();

            viewModel.PortfolioDescription.Should().Be("You have 100 MSFT shares");
        }

        [TestMethod]
        public void Adding_invalid_asset_should_set_error_message()
        {
            var portfolioStore = new PortfolioStore();
            var viewModel = new MainViewModel(portfolioStore);
            viewModel.Load();

            viewModel.ErrorMessage.Should().BeNull();

            using (IMonitor<MainViewModel> viewModelMonitored = viewModel.Monitor())
            {
                viewModel.NewAssetSymbol = "MSFT";
                viewModel.NewAssetAmount = -10;
                viewModel.AddAsset();

                viewModelMonitored.Should().RaisePropertyChangeFor(vm => vm.ErrorMessage);
                viewModelMonitored.Should().NotRaisePropertyChangeFor(vm => vm.PortfolioDescription);
            }
        }

        private static void CheckIsStaticMethodCall(LambdaExpression expression, Type methodDeclaringType, string methodName)
        {
            expression.Body.Should().BeAssignableTo<MethodCallExpression>();
            var methodCallExpression = expression.Body as MethodCallExpression;
            methodCallExpression.Object.Should().BeNull();
            methodCallExpression.Method.DeclaringType.Should().Be(methodDeclaringType);
            methodCallExpression.Method.Name.Should().Be(methodName);
        }
    }
}