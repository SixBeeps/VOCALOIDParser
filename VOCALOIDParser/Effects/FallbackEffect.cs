using System.Text.Json.Nodes;

namespace SixBeeps.VOCALOIDParser.Effects
{
    public class FallbackEffect : Effect
    {
        public Dictionary<string, object> Parameters { get; set; }

        public FallbackEffect(JsonNode json) : base(json)
        {
            Parameters = new();
            foreach(JsonNode p in paramList)
            {
                Parameters.Add(p["name"].GetValue<string>(), p["value"].GetValue<object>());
            }
        }
    }
}