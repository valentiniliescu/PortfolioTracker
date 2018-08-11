using JetBrains.Annotations;

namespace PortfolioTracker.Model
{
    public sealed class Symbol
    {
        public Symbol([NotNull] string text)
        {
            Text = text.ToUpperInvariant().Trim();
        }

        [NotNull]
        public string Text { get; }

        public override bool Equals(object obj)
        {
            return obj is Symbol otherSymbol
                   && Text == otherSymbol.Text;
        }

        public override int GetHashCode() => Text.GetHashCode();
    }
}