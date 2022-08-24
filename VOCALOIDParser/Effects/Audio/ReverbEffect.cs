using System.Text.Json.Nodes;

namespace SixBeeps.VOCALOIDParser.Effects
{
    public class ReverbEffect : Effect
    {
        /// <summary>
        /// Initial delay of the reverberated signal.
        /// </summary>
        public float Offset { get; set; }

        /// <summary>
        /// How much of the wet signal to use.
        /// </summary>
        public float Mix { get; set; }

        /// <summary>
        /// How long the reverberation lasts.
        /// </summary>
        public float Duration { get; set; }

        /// <summary>
        /// The type of reverb used (Hall, Room, and Plate)
        /// </summary>
        public float ReverbType { get; set; }

        public ReverbEffect(JsonNode json) : base(json)
        {
            Offset = GetParamValueByName<float>("Initial Delay");
            Mix = GetParamValueByName<float>("Mix");
            Duration = GetParamValueByName<float>("Reverb Time");
            ReverbType = GetParamValueByName<float>("Type");
        }
    }
}