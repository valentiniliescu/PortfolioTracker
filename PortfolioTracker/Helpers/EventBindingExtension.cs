using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;
using JetBrains.Annotations;

namespace PortfolioTracker.Helpers
{
    // allows to bind WPF events directly to ViewModel methods
    // inspired by https://www.thomaslevesque.com/2011/09/23/wpf-4-5-subscribing-to-an-event-using-a-markup-extension/
    public sealed class EventBindingExtension : MarkupExtension
    {
// could not find a better way to retrieve the event bindings in unit tests
#if DEBUG
        [CanBeNull] public static ConcurrentBag<Tuple<string, string, string>> EventBindingStore;
#endif

        public EventBindingExtension([NotNull] string methodName)
        {
            MethodName = methodName;
        }

        [NotNull]
        private string MethodName { get; }

        [Pure]
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var target = (IProvideValueTarget) serviceProvider.GetService(typeof(IProvideValueTarget));
            if (target == null)
            {
                throw new InvalidOperationException("Invalid method binding target");
            }

            var eventInfo = target.TargetProperty as EventInfo;
            if (eventInfo == null)
            {
                throw new InvalidOperationException("The target property of the method binding must be an event");
            }

            var targetDependencyObject = target.TargetObject as DependencyObject;
            if (!(targetDependencyObject is FrameworkElement) && !(targetDependencyObject is FrameworkContentElement))
            {
                throw new InvalidOperationException("The method binding target must be a FrameworkElement or a FrameworkContentElement");
            }

            // ReSharper disable AssignNullToNotNullAttribute
            object dataContext = targetDependencyObject.GetValue(FrameworkElement.DataContextProperty) ?? targetDependencyObject.GetValue(FrameworkContentElement.DataContextProperty);
            // ReSharper restore AssignNullToNotNullAttribute
            if (dataContext == null)
            {
                throw new InvalidOperationException("No DataContext found on method binding target");
            }

            object handler = GetHandler(dataContext, eventInfo, MethodName);
            if (handler == null)
            {
                throw new ArgumentException("No valid instance parameterless method with the specified name that returns void was found for method binding.", nameof(MethodName));
            }

#if DEBUG
            // ReSharper disable AssignNullToNotNullAttribute
            EventBindingStore?.Add(new Tuple<string, string, string>(
                (targetDependencyObject.GetValue(FrameworkElement.NameProperty) ?? targetDependencyObject.GetValue(FrameworkContentElement.NameProperty))?.ToString(),
                eventInfo.Name,
                MethodName));
            // ReSharper restore AssignNullToNotNullAttribute
#endif

            return handler;
        }

        [Pure]
        private static object GetHandler([NotNull] object dataContext, [NotNull] EventInfo eventInfo, [NotNull] string eventHandlerName)
        {
            Type dataContextType = dataContext.GetType();

            MethodInfo method = dataContextType.GetMethod(eventHandlerName);

            if (method != null && !method.IsStatic && method.GetParameters().Length == 0 && method.ReturnType == typeof(void))
            {
                EventHandler handler = (sender, args) => { method.Invoke(dataContext, new object[0]); };

                return Delegate.CreateDelegate(eventInfo.EventHandlerType, handler.Target, handler.Method);
            }

            return null;
        }
    }
}