namespace PortfolioTracker.Model
{
    public sealed class Quote
    {
        public readonly decimal Price;
        public readonly Symbol Symbol;

        public Quote(Symbol symbol, decimal price)
        {
            Symbol = symbol;
            Price = price;
        }
    }
}