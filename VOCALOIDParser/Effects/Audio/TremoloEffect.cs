using System.Text.Json.Nodes;

namespace SixBeeps.VOCALOIDParser.Effects
{
    public class TremoloEffect : Effect
    {
        /// <summary>
        /// Frequency of the tremolo when synced by seconds.
        /// </summary>
        public float LFOFrequency { get; set; }

        /// <summary>
        /// Amount of modulation applied to the amplitude.
        /// </summary>
        public float AMDepth;

        /// <summary>
        /// Amount of modulation applied to the pitch.
        /// </summary>
        public float PMDepth;

        /// <summary>
        /// Whether or not the tremolo is synced by tempo or seconds.
        /// </summary>
        public float TempoSync { get; set; }

        /// <summary>
        /// Frequency of the tremolo when tempo-synced.
        /// </summary>
        public float SyncNote { get; set; }

        /// <summary>
        /// The shape of the waveform used by the tremolo (square and sine).
        /// </summary>
        public float WaveShape { get; set; }

        public TremoloEffect(JsonNode json) : base(json)
        {
            LFOFrequency = GetParamValueByName<float>("LFO Freq");
            AMDepth = GetParamValueByName<float>("AM Depth");
            PMDepth = GetParamValueByName<float>("PM Depth");
            TempoSync = GetParamValueByName<float>("Tempo Sync");
            SyncNote = GetParamValueByName<float>("Sync Note");
            WaveShape = GetParamValueByName<float>("Shape Type");
        }
    }
}