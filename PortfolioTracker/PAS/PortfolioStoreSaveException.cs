using System;
using JetBrains.Annotations;

namespace PortfolioTracker.PAS
{
    public sealed class PortfolioStoreSaveException : Exception
    {
        public PortfolioStoreSaveException([NotNull] string message, [NotNull] Exception innerException) : base(message, innerException)
        {
        }
    }
}