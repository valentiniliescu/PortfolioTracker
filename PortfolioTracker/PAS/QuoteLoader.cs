using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using PortfolioTracker.Model;

namespace PortfolioTracker.PAS
{
    public static class QuoteLoader
    {
        [NotNull] private static readonly HttpClient HttpClient = new HttpClient();

        [NotNull]
        [ItemNotNull]
        [Pure]
        public static async Task<IEnumerable<Quote>> Load([NotNull] [ItemNotNull] IEnumerable<Symbol> symbols)
        {
            string symbolTextList = string.Join(",", symbols.Select(symbol => symbol.Text));
            var uri = new Uri($"https://api.iextrading.com/1.0/stock/market/batch?symbols={symbolTextList}&types=price");

            // ReSharper disable once PossibleNullReferenceException
            string json = await HttpClient.GetStringAsync(uri);

            JObject jsonObject = JObject.Parse(json);

            // ReSharper disable AssignNullToNotNullAttribute
            // ReSharper disable PossibleNullReferenceException
            return jsonObject.Properties().Select(property => new Quote(new Symbol(property.Name), decimal.Parse(((JObject) property.Value)["price"].Value<string>())));
            // ReSharper restore PossibleNullReferenceException
            // ReSharper restore AssignNullToNotNullAttribute
        }

        // TODO: add error handling
    }
}