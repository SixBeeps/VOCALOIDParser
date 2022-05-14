#pragma warning disable CS8602 // Dereference of a possibly null reference.
using System.Text.Json.Nodes;

namespace SixBeeps.VOCALOIDParser.Effects
{
    public class BreathEffect : Effect
    {
        /// <summary>
        /// How hard the breaths taken are.
        /// </summary>
        public float Exhalation { get; set; }

        /// <summary>
        /// How solid the breaths taken are.
        /// </summary>
        public float Mode { get; set; }

        /// <summary>
        /// The gender of the breath.
        /// </summary>
        public float Gender { get; set; }

        public BreathEffect(JsonNode json) : base(json)
        {
            Exhalation = GetParamValueByName<int>("Exhalation");
            Mode = GetParamValueByName<int>("Mode");
            Gender = GetParamValueByName<int>("Type");
        }
    }
}
#pragma warning restore CS8602 // Dereference of a possibly null reference.