#pragma warning disable CS8602 // Dereference of a possibly null reference.
using System.Text.Json.Nodes;

namespace SixBeeps.VOCALOIDParser.Effects
{
    public class DeEsserEffect : Effect
    {
        /// <summary>
        /// Frequency of the S sound.
        /// </summary>
        public float DetectFrequency { get; set; }

        /// <summary>
        /// Volume threshold for when to start limiting.
        /// </summary>
        public float Threshold { get; set; }

        /// <summary>
        /// Whether the S sound should be removed or soloed.
        /// </summary>
        public float Monitor { get; set; }

        public DeEsserEffect(JsonNode json) : base(json)
        {
            DetectFrequency = GetParamValueByName<float>("Detect Freq");
            Threshold = GetParamValueByName<float>("Threshold");
            Monitor = GetParamValueByName<float>("Monitor");
        }
    }
}
#pragma warning restore CS8602 // Dereference of a possibly null reference.