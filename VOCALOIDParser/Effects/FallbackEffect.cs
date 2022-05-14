#pragma warning disable CS8602 // Dereference of a possibly null reference.
using System.Text.Json.Nodes;

namespace SixBeeps.VOCALOIDParser.Effects
{
    public class FallbackEffect : Effect
    {
        public Dictionary<string, float> Parameters { get; set; }

        public FallbackEffect(JsonNode json) : base(json)
        {
            Parameters = new();
            foreach(JsonNode p in paramList)
            {
                Parameters.Add(p["name"].GetValue<string>(), p["value"].GetValue<float>());
            }
        }
    }
}
#pragma warning restore CS8602 // Dereference of a possibly null reference.