using System;
using JetBrains.Annotations;

namespace PortfolioTracker
{
    internal static class Program
    {
        private static void Main()
        {
            Console.WriteLine(HelloWorld());
        }

        [Pure]
        private static string HelloWorld()
        {
            return "Hello " + "world!";
        }
    }
}