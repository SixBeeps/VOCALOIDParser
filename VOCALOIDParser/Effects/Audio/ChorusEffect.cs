using System.Text.Json.Nodes;

namespace SixBeeps.VOCALOIDParser.Effects
{
    public class ChorusEffect : Effect
    {
        /// <summary>
        /// Frequency of the chorus LFO when synced by seconds.
        /// </summary>
        public float LFOFrequency { get; set; }

        /// <summary>
        /// Depth of the chorus effect.
        /// </summary>
        public float Depth { get; set; }

        /// <summary>
        /// Feedback of the chorus effect.
        /// </summary>
        public float Feedback { get; set; }

        /// <summary>
        /// Initial delay of the chorused signal.
        /// </summary>
        public float Offset { get; set; }

        /// <summary>
        /// Stereo separation of the chorused signal.
        /// </summary>
        public float Spatial { get; set; }

        /// <summary>
        /// How much of the dry and wet signal to use.
        /// </summary>
        public float DryWet { get; set; }

        /// <summary>
        /// Whether or not the LFO is synced by tempo or seconds.
        /// </summary>
        public float TempoSync { get; set; }

        /// <summary>
        /// Frequency of the chorus LFO when tempo-synced.
        /// </summary>
        public float SyncNote { get; set; }

        /// <summary>
        /// The type of chorus used (Chorus 1, Chorus 2, and Flanger).
        /// </summary>
        public float ChorusType { get; set; }

        public ChorusEffect(JsonNode json) : base(json)
        {
            LFOFrequency = GetParamValueByName<float>("LFO Freq");
            Depth = GetParamValueByName<float>("Depth");
            Feedback = GetParamValueByName<float>("Feedback");
            Offset = GetParamValueByName<float>("Delay Offset");
            Spatial = GetParamValueByName<float>("Spatial");
            DryWet = GetParamValueByName<float>("Dry/Wet");
            TempoSync = GetParamValueByName<float>("Tempo Sync");
            SyncNote = GetParamValueByName<float>("Sync Note");
            ChorusType = GetParamValueByName<float>("Type");
        }
    }
}