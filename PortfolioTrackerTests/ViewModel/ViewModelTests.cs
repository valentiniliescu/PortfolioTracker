using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    public class ViewModelTests
    {
        [TestMethod]
        public void Portfolio_description_should_call_formatter()
        {
            var viewModel = new MainViewModel();

            LambdaExpression fromPropertyGetter = LambdaExpressionConverter.FromPropertyGetter(viewModel, nameof(viewModel.PortfolioDescription));

            CheckIsStaticMethodCallOnProperty(fromPropertyGetter, typeof(PortfolioFormatter), nameof(PortfolioFormatter.Format), viewModel.GetType(), nameof(viewModel.Portfolio));
        }

        [TestMethod]
        public void Portfolio_value_description_should_call_formatter()
        {
            var viewModel = new MainViewModel();

            LambdaExpression fromPropertyGetter = LambdaExpressionConverter.FromPropertyGetter(viewModel, nameof(viewModel.PortfolioValueDescription));

            CheckIsStaticMethodCallOnProperty(fromPropertyGetter, typeof(PortfolioValueFormatter), nameof(PortfolioValueFormatter.Format), viewModel.GetType(), nameof(viewModel.TotalValue));
        }

        [TestMethod]
        public async Task Loading_assets_should_change_the_portfolio_and_fire_property_changed_event()
        {
            var viewModel = new MainViewModel();

            viewModel.Portfolio.Should().BeNull();

            using (IMonitor<MainViewModel> viewModelMonitored = viewModel.Monitor())
            {
                await viewModel.Load();

                viewModelMonitored.Should().RaisePropertyChangeFor(vm => vm.PortfolioDescription);
                viewModel.Portfolio.Assets.Should().BeEmpty();

                viewModelMonitored.Should().RaisePropertyChangeFor(vm => vm.PortfolioValueDescription);
                viewModel.TotalValue.Should().Be(0);
            }
        }

        [TestMethod]
        public async Task Adding_an_asset_should_change_the_portfolio_assets_and_fire_property_changed_event()
        {
            QuoteLoaderDelegate quoteLoaderPrice100 = symbols => Task.FromResult(symbols.Select(symbol => new Quote(symbol, 100)));

            var viewModel = new MainViewModel(new PortfolioWithValue(quoteLoaderPrice100));
            await viewModel.Load();

            using (IMonitor<MainViewModel> viewModelMonitored = viewModel.Monitor())
            {
                viewModel.NewAssetSymbol = "MSFT";
                viewModel.NewAssetAmount = 100;
                await viewModel.AddAsset();

                viewModelMonitored.Should().RaisePropertyChangeFor(vm => vm.PortfolioDescription);
                viewModel.Portfolio.Assets.Should().BeEquivalentTo(new Asset(new Symbol("MSFT"), 100));

                viewModelMonitored.Should().RaisePropertyChangeFor(vm => vm.PortfolioValueDescription);
                viewModel.TotalValue.Should().Be(10000);
            }
        }

        [TestMethod]
        public async Task Saving_should_save_the_assets_till_next_load()
        {
            var viewModel = new MainViewModel();
            await viewModel.Load();

            viewModel.NewAssetSymbol = "MSFT";
            viewModel.NewAssetAmount = 100;
            await viewModel.AddAsset();

            await viewModel.Save();

            viewModel.NewAssetSymbol = "AAPL";
            viewModel.NewAssetAmount = 10;
            await viewModel.AddAsset();

            await viewModel.Load();

            viewModel.Portfolio.Assets.Should().BeEquivalentTo(new Asset(new Symbol("MSFT"), 100));
        }

        [TestMethod]
        public async Task Adding_invalid_asset_should_set_error_message()
        {
            var viewModel = new MainViewModel();
            await viewModel.Load();

            viewModel.ErrorMessage.Should().BeNull();

            using (IMonitor<MainViewModel> viewModelMonitored = viewModel.Monitor())
            {
                viewModel.NewAssetSymbol = "MSFT";
                viewModel.NewAssetAmount = -10;
                await viewModel.AddAsset();

                viewModelMonitored.Should().RaisePropertyChangeFor(vm => vm.ErrorMessage);
                viewModel.ErrorMessage.Should().NotBeNull();
            }
        }

        [TestMethod]
        public async Task Store_loading_error_should_set_error_message()
        {
            var portfolioStoreWithThrowOnLoad = new InMemoryPortfolioStore {ThrowOnLoad = true};
            var viewModel = new MainViewModel(new PortfolioWithValue(portfolioStoreWithThrowOnLoad));

            using (IMonitor<MainViewModel> viewModelMonitored = viewModel.Monitor())
            {
                await viewModel.Load();

                viewModelMonitored.Should().RaisePropertyChangeFor(vm => vm.ErrorMessage);
                viewModel.ErrorMessage.Should().NotBeNull();
            }
        }

        [TestMethod]
        public async Task Store_saving_error_should_set_error_message()
        {
            var portfolioStoreWithThrowOnSave = new InMemoryPortfolioStore {ThrowOnSave = true};
            var viewModel = new MainViewModel(new PortfolioWithValue(portfolioStoreWithThrowOnSave));

            using (IMonitor<MainViewModel> viewModelMonitored = viewModel.Monitor())
            {
                await viewModel.Save();

                viewModelMonitored.Should().RaisePropertyChangeFor(vm => vm.ErrorMessage);
                viewModel.ErrorMessage.Should().NotBeNull();
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