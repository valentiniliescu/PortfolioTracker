using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DelegateDecompiler;
using JetBrains.Annotations;

namespace PortfolioTrackerTests.Helpers
{
    internal static class LambdaExpressionConverter
    {
        public static LambdaExpression FromPropertyGetter([NotNull] object target, [NotNull] string propertyName)
        {
            PropertyInfo propertyInfo = target.GetType().GetProperty(propertyName);

            if (propertyInfo == null)
            {
                throw new InvalidOperationException($"Property {propertyName} not found on object {target}");
            }

            CheckExcludedFromCodeCoverage(propertyInfo);

            MethodInfo propertyGetterMethodInfo = propertyInfo.GetGetMethod();

            if (propertyGetterMethodInfo == null)
            {
                throw new InvalidOperationException($"Property {propertyName} does not have a getter");
            }

            Type delegateType = typeof(Func<>).MakeGenericType(propertyGetterMethodInfo.ReturnType);

            Delegate propertyGetterDelegate = Delegate.CreateDelegate(delegateType, target, propertyGetterMethodInfo);

            return propertyGetterDelegate.Decompile();
        }

        public static LambdaExpression FromInstanceMethod([NotNull] object target, [NotNull] string methodName)
        {
            MethodInfo methodInfo = target.GetType().GetMethod(methodName);

            if (methodInfo == null)
            {
                throw new InvalidOperationException($"Method {methodName} not found on object {target}");
            }

            Type methodReturnType = methodInfo.ReturnType;

            if (methodReturnType == typeof(void))
            {
                throw new InvalidOperationException("Methods that return void are not supported");
            }

            Type[] argumentsAndReturnTypes = methodInfo.GetParameters()
                .Select(parameterInfo => parameterInfo?.ParameterType).Concat(new[] {methodReturnType}).ToArray();

            Type funcType = GetFuncType(argumentsAndReturnTypes.Length);

            Type delegateType = funcType.MakeGenericType(argumentsAndReturnTypes);

            Delegate methodDelegate = Delegate.CreateDelegate(delegateType, target, methodInfo);

            return methodDelegate.Decompile();
        }

        [NotNull]
        private static Type GetFuncType(int argumentsAndReturnTypesCount)
        {
            switch (argumentsAndReturnTypesCount)
            {
                case 1:
                    return typeof(Func<>);
                case 2:
                    return typeof(Func<,>);
                case 3:
                    return typeof(Func<,,>);
                case 4:
                    return typeof(Func<,,,>);
                case 5:
                    return typeof(Func<,,,,>);
                case 6:
                    return typeof(Func<,,,,,>);
                default:
                    throw new NotImplementedException();
            }
        }

        private static void CheckExcludedFromCodeCoverage([NotNull] MemberInfo propertyInfo)
        {
            if (propertyInfo.GetCustomAttribute<ExcludeFromCodeCoverageAttribute>() == null)
            {
                throw new InvalidOperationException("LambdaExpressionConverter should not be used on members under code coverage, since it interferes with decompilation");
            }
        }
    }
}