using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using FluentAssertions;
using FluentAssertions.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PortfolioTracker.Model;
using PortfolioTracker.PAS;
using PortfolioTracker.ViewModel;
using PortfolioTrackerTests.Helpers;

namespace PortfolioTrackerTests.ViewModel
{
    [TestClass]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public class ViewModelTests
    {
        [TestMethod]
        public void Portfolio_description_should_call_formatter()
        {
            var viewModel = new MainViewModel(new InMemoryPortfolioStore());

            LambdaExpression fromPropertyGetter = LambdaExpressionConverter.FromPropertyGetter(viewModel, nameof(viewModel.PortfolioDescription));

            CheckIsStaticMethodCallOnProperty(fromPropertyGetter, typeof(PortfolioFormatter), nameof(PortfolioFormatter.Format), viewModel.GetType(), nameof(viewModel.Portfolio));
        }

        [TestMethod]
        public void Loading_assets_should_change_the_portfolio_and_fire_property_changed_event()
        {
            var viewModel = new MainViewModel(new InMemoryPortfolioStore());

            viewModel.Portfolio.Should().BeNull();

            using (IMonitor<MainViewModel> viewModelMonitored = viewModel.Monitor())
            {
                viewModel.Load();

                viewModelMonitored.Should().RaisePropertyChangeFor(vm => vm.PortfolioDescription);

                viewModel.Portfolio.Assets.Should().BeEmpty();
            }
        }

        [TestMethod]
        public void Adding_an_asset_should_change_the_portfolio_assets_and_fire_property_changed_event()
        {
            var viewModel = new MainViewModel(new InMemoryPortfolioStore());
            viewModel.Load();

            using (IMonitor<MainViewModel> viewModelMonitored = viewModel.Monitor())
            {
                viewModel.NewAssetSymbol = "MSFT";
                viewModel.NewAssetAmount = 100;
                viewModel.AddAsset();

                viewModelMonitored.Should().RaisePropertyChangeFor(vm => vm.PortfolioDescription);
                viewModel.Portfolio.Assets.Should().BeEquivalentTo(new Asset(new Symbol("MSFT"), 100));
            }
        }

        [TestMethod]
        public void Saving_should_save_the_assets_till_next_load()
        {
            var viewModel = new MainViewModel(new InMemoryPortfolioStore());
            viewModel.Load();

            viewModel.NewAssetSymbol = "MSFT";
            viewModel.NewAssetAmount = 100;
            viewModel.AddAsset();

            viewModel.Save();

            viewModel.NewAssetSymbol = "AAPL";
            viewModel.NewAssetAmount = 10;
            viewModel.AddAsset();

            viewModel.Load();

            viewModel.Portfolio.Assets.Should().BeEquivalentTo(new Asset(new Symbol("MSFT"), 100));
        }

        [TestMethod]
        public void Adding_invalid_asset_should_set_error_message()
        {
            var viewModel = new MainViewModel(new InMemoryPortfolioStore());
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

        private static void CheckIsStaticMethodCallOnProperty(LambdaExpression expression, Type methodDeclaringType, string methodName, Type propertyDeclaringType, string propertyName)
        {
            expression.Body.Should().BeAssignableTo<MethodCallExpression>();
            var methodCallExpression = expression.Body as MethodCallExpression;
            methodCallExpression.Object.Should().BeNull();
            methodCallExpression.Method.DeclaringType.Should().Be(methodDeclaringType);
            methodCallExpression.Method.Name.Should().Be(methodName);

            methodCallExpression.Arguments.Should().HaveCount(1);
            methodCallExpression.Arguments[0].Should().BeAssignableTo<MemberExpression>();
            var memberExpression = methodCallExpression.Arguments[0] as MemberExpression;
            memberExpression.Member.DeclaringType.Should().Be(propertyDeclaringType);
            memberExpression.Member.Name.Should().Be(propertyName);
        }
    }
}