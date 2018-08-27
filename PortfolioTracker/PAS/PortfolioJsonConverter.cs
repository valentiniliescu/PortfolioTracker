using System;
using JetBrains.Annotations;
using Newtonsoft.Json;
using PortfolioTracker.Model;

namespace PortfolioTracker.PAS
{
    public sealed class PortfolioJsonConverter : JsonConverter<Portfolio>
    {
        public override void WriteJson(JsonWriter writer, Portfolio portfolio, [NotNull] JsonSerializer serializer)
        {
            serializer.Serialize(writer, portfolio?.Assets);
        }

        public override Portfolio ReadJson(JsonReader reader, Type objectType, Portfolio existingValue, bool hasExistingValue, [NotNull] JsonSerializer serializer)
        {
            var assets = serializer.Deserialize<Asset[]>(reader);

            return new Portfolio(assets);
        }
    }
}