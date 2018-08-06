using JetBrains.Annotations;

namespace PortfolioTracker.ViewModel
{
    public static class PortfolioValueFormatter
    {
        [Pure]
        [CanBeNull]
        [MustUseReturnValue]
        public static string Format(decimal value)
        {
            if (value == 0)
            {
                return null;
            }

            return $"Total value ${value}";
        }
    }
}