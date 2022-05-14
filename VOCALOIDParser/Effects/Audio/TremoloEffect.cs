#pragma warning disable CS8602 // Dereference of a possibly null reference.
using System.Text.Json.Nodes;

namespace SixBeeps.VOCALOIDParser.Effects
{
    public class TremoloEffect : Effect
    {
        /// <summary>
        /// Frequency of the tremolo.
        /// </summary>
        public float LFOFrequency { get; set; }

        /// <summary>
        /// TODO: Figure out what these values are
        /// </summary>
        public float AMDepth, PMDepth;

        /// <summary>
        /// Whether or not the tremolo is synced by tempo or seconds.
        /// </summary>
        public float TempoSync { get; set; }

        /// <summary>
        /// TODO: Figure out what this value means
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
#pragma warning restore CS8602 // Dereference of a possibly null reference.