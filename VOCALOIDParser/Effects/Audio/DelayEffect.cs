using System.Text.Json.Nodes;

namespace SixBeeps.VOCALOIDParser.Effects
{
    public class DelayEffect : Effect
    {
        /// <summary>
        /// How much of the dry and wet signal to use.
        /// </summary>
        public float DryWet { get; set; }

        /// <summary>
        /// How much high damping is applied.
        /// </summary>
        public float HighDamping { get; set; }

        /// <summary>
        /// How the separate channels interact with each other (Stereo and Cross)
        /// </summary>
        public float StereoMode { get; set; }

        /// <summary>
        /// Stereo separation of the chorused signal.
        /// </summary>
        public float Spatial { get; set; }

        /// <summary>
        /// Whether or not the LFO is synced by tempo or seconds.
        /// </summary>
        public float TempoSync { get; set; }

        /// <summary>
        /// Individual stereo channels of the delay effect.
        /// </summary>
        public DelayChannel Left, Right;

        public DelayEffect(JsonNode json) : base(json)
        {
            DryWet = GetParamValueByName<float>("Dry/Wet");
            HighDamping = GetParamValueByName<float>("High Damp");
            StereoMode = GetParamValueByName<float>("Mode");
            Spatial = GetParamValueByName<float>("Spatial");
            TempoSync = GetParamValueByName<float>("Tempo Sync");
            Left = new DelayChannel(
                GetParamValueByName<float>("Lch Delay1"),
                GetParamValueByName<float>("Lch FB Gain"),
                GetParamValueByName<float>("Lch Sync Note")
            );
            Right = new DelayChannel(
                GetParamValueByName<float>("Rch Delay1"),
                GetParamValueByName<float>("Rch FB Gain"),
                GetParamValueByName<float>("Rch Sync Note")
            );
        }
    }

    public struct DelayChannel
    {
        /// <summary>
        /// Time to delay this channel's signal
        /// </summary>
        public float Delay { get; set; }

        /// <summary>
        /// Feedback of the delay channel.
        /// </summary>
        public float Feedback { get; set; }

        /// <summary>
        /// TODO: Still don't know what this does.
        /// </summary>
        public float SyncNote { get; set; }

        public DelayChannel(float delay, float feedback, float syncNote)
        {
            Delay = delay;
            Feedback = feedback;
            SyncNote = syncNote;
        }
    }
}