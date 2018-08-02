using System;
using JetBrains.Annotations;

namespace PortfolioTracker.PAS
{
    public class PortfolioStoreSaveException : Exception
    {
        public PortfolioStoreSaveException([NotNull] string message, [NotNull] Exception innerException) : base(message, innerException)
        {
        }
    }
}