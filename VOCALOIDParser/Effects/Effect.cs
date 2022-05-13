#pragma warning disable CS8602 // Dereference of a possibly null reference.
using System.Text.Json.Nodes;

namespace SixBeeps.VOCALOIDParser.Effects
{
    public class Effect
    {
        internal JsonArray paramList;

        /// <summary>
        /// ID of the effect. This may or may not be human-readable depending on the effect.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Whether or not this effect is bypassed/disabled.
        /// </summary>
        public bool Bypassed { get; set; }

        /// <summary>
        /// Whether or not this effect is folded.
        /// </summary>
        public bool Folded { get; set; }

        public Effect(JsonNode json)
        {
            paramList = json["Parameters"].AsArray();
            Id = json["id"].GetValue<string>();
            Bypassed = json["isBypassed"].GetValue<bool>();
            Folded = json["isFolded"].GetValue<bool>();
        }

        protected T GetParamValueByName<T>(string name)
        {
            foreach (JsonNode parameter in paramList)
            {
                if (parameter["name"].GetValue<string>() == name)
                {
                    return parameter["value"].GetValue<T>();
                }
            }

            throw new ArgumentException($"Failed to find parameter name \"{name}\" in effect {Id}");
        }
    }
}
#pragma warning restore CS8602 // Dereference of a possibly null reference.