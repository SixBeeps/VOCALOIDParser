using System.Text.Json.Nodes;

namespace SixBeeps.VOCALOIDParser.Effects
{
    public class DistortionEffect : Effect
    {
        /// <summary>
        /// How much to distort the signal.
        /// </summary>
        public float Drive { get; set; }

        /// <summary>
        /// Type of Lo-Fi to use.
        /// </summary>
        public float LoFiType { get; set; }

        /// <summary>
        /// How much Lo-Fi to apply.
        /// </summary>
        public float LoFiAmount { get; set; }

        /// <summary>
        /// Gain of the post signal.
        /// </summary>
        public float Output { get; set; }

        public DistortionEffect(JsonNode json) : base(json)
        {
            Drive = GetParamValueByName<float>("Drive");
            LoFiType = GetParamValueByName<float>("Lo-Fi Type");
            LoFiAmount = GetParamValueByName<float>("Lo-Fi Amount");
            Output = GetParamValueByName<float>("Output");
        }
    }
}