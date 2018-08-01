using System;
using System.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json;
using PortfolioTracker.Model;

namespace PortfolioTracker.PAS
{
    public class PortfolioJsonConverter : JsonConverter<Portfolio>
    {
        public override void WriteJson(JsonWriter writer, Portfolio portfolio, [NotNull] JsonSerializer serializer)
        {
            serializer.Serialize(writer, portfolio?.Assets);
        }

        public override Portfolio ReadJson(JsonReader reader, Type objectType, Portfolio existingValue, bool hasExistingValue, [NotNull] JsonSerializer serializer)
        {
            var portfolio = new Portfolio();

            var assets = serializer.Deserialize<Asset[]>(reader);

            assets?
                .Where(asset => asset != null)
                .ToList()
                .ForEach(portfolio.AddAsset);

            return portfolio;
        }
    }
}