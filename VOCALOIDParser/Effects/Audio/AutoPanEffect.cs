using System.Text.Json.Nodes;

namespace SixBeeps.VOCALOIDParser.Effects
{
    public class AutoPanEffect : Effect
    {
        /// <summary>
        /// Frequency of the autopan.
        /// </summary>
        public float LFOFrequency { get; set; }

        /// <summary>
        /// How far the autopan pans the signal.
        /// </summary>
        public float Depth;

        /// <summary>
        /// Whether or not the autopan is synced by tempo or seconds.
        /// </summary>
        public float TempoSync { get; set; }

        /// <summary>
        /// TODO: Figure out what this value means
        /// </summary>
        public float SyncNote { get; set; }

        /// <summary>
        /// The shape of the waveform used by the autopan (square and sine).
        /// </summary>
        public float WaveShape { get; set; }

        public AutoPanEffect(JsonNode json) : base(json)
        {
            LFOFrequency = GetParamValueByName<float>("LFO Freq");
            Depth = GetParamValueByName<float>("Depth");
            TempoSync = GetParamValueByName<float>("Tempo Sync");
            SyncNote = GetParamValueByName<float>("Sync Note");
            WaveShape = GetParamValueByName<float>("Shape Type");
        }
    }
}