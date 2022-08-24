using System.Text.Json.Nodes;

namespace SixBeeps.VOCALOIDParser.Effects
{
    public class GainEffect : Effect
    {
        /// <summary>
        /// How much to change the signal volume.
        /// </summary>
        public float GainAmount { get; set; }

        public GainEffect(JsonNode json) : base(json)
        {
            GainAmount = GetParamValueByName<float>("Gain");
        }
    }
}