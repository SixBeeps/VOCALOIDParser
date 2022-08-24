using System.Text.Json.Nodes;

namespace SixBeeps.VOCALOIDParser.Effects
{
    public class CompressorEffect : Effect
    {
        /// <summary>
        /// Volume threshold for when to start compressing.
        /// </summary>
        public float Threshold { get; set; }

        /// <summary>
        /// Attack time until full compression.
        /// </summary>
        public float Attack { get; set; }

        /// <summary>
        /// Release time after full compression.
        /// </summary>
        public float Release { get; set; }

        /// <summary>
        /// How much the compressor acts on the volume.
        /// </summary>
        public float Ratio { get; set; }

        /// <summary>
        /// General shape of the compressor.
        /// </summary>
        public float Knee { get; set; }

        public CompressorEffect(JsonNode json) : base(json)
        {
            Threshold = GetParamValueByName<float>("Threshold");
            Attack = GetParamValueByName<float>("Attack");
            Release = GetParamValueByName<float>("Release");
            Ratio = GetParamValueByName<float>("Ratio");
            Knee = GetParamValueByName<float>("Knee");
        }
    }
}