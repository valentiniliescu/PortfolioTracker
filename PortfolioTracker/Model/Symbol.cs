using JetBrains.Annotations;

namespace PortfolioTracker.Model
{
    public class Symbol
    {
        public Symbol([NotNull] string text)
        {
            Text = text.ToUpperInvariant().Trim();
        }

        [NotNull]
        public string Text { get; }

        public override bool Equals(object obj)
        {
            var otherSymbol = obj as Symbol;

            return otherSymbol != null
                   && Text == otherSymbol.Text;
        }

        public override int GetHashCode() => Text.GetHashCode();
    }
}