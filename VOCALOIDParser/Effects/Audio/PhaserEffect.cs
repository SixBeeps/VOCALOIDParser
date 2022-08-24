using System.Text.Json.Nodes;

namespace SixBeeps.VOCALOIDParser.Effects
{
    public class PhaserEffect : Effect
    {
        /// <summary>
        /// Frequency of the phaser LFO.
        /// </summary>
        public float LFOFrequency { get; set; }

        /// <summary>
        /// Depth of the phaser effect.
        /// </summary>
        public float Depth { get; set; }

        /// <summary>
        /// Delay of the phased signal.
        /// </summary>
        public float DelayOffset { get; set; }

        /// <summary>
        /// Feedback of the phaser effect.
        /// </summary>
        public float Feedback { get; set; }

        /// <summary>
        /// How much of the dry and wet signal to use.
        /// </summary>
        public float DryWet { get; set; }

        /// <summary>
        /// Stereo separation (?) of the phased signal.
        /// </summary>
        public float Stage { get; set; }

        /// <summary>
        /// Whether or not the LFO is synced by tempo or seconds.
        /// </summary>
        public float TempoSync { get; set; }

        /// <summary>
        /// TODO: Figure out what this value means
        /// </summary>
        public float SyncNote { get; set; }

        /// <summary>
        /// The type of phaser used (Phaser 1 and Phaser 2).
        /// </summary>
        public float PhaserType { get; set; }

        public PhaserEffect(JsonNode json) : base(json)
        {
            LFOFrequency = GetParamValueByName<float>("Rate");
            Depth = GetParamValueByName<float>("Depth");
            DelayOffset = GetParamValueByName<float>("Phase Offset");
            Feedback = GetParamValueByName<float>("Feedback Level");
            DryWet = GetParamValueByName<float>("Dry/Wet");
            Stage = GetParamValueByName<float>("Stage");
            TempoSync = GetParamValueByName<float>("Tempo Sync");
            SyncNote = GetParamValueByName<float>("Sync Note");
            PhaserType = GetParamValueByName<float>("Type");
        }
    }
}