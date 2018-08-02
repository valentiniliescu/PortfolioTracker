using System;
using JetBrains.Annotations;

namespace PortfolioTracker.PAS
{
    public class PortfolioStoreLoadException : Exception
    {
        public PortfolioStoreLoadException([NotNull] string message, [NotNull] Exception innerException) : base(message, innerException)
        {
        }
    }
}