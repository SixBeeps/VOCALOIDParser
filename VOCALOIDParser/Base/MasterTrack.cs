using System.Text.Json.Nodes;
using SixBeeps.VOCALOIDParser.Effects;

namespace SixBeeps.VOCALOIDParser
{
    public class MasterTrack
    {
        /// <summary>
        /// Audio sampling rate in hertz. Changing this is not recommended.
        /// </summary>
        public int SamplingRate { get; set; }

        /// <summary>
        /// Whether or not looping is enabled.
        /// </summary>
        public bool LoopEnabled { get; set; }

        /// <summary>
        /// Looping range.
        /// </summary>
        public TimeRange LoopRange { get; set; }

        /// <summary>
        /// Tempo track for the whole song.
        /// </summary>
        public GlobalAutomationTrack TempoTrack { get; set; }

        /// <summary>
        /// Volume track for the whole song.
        /// </summary>
        public AutomationTrack VolumeTrack { get; set; }

        /// <summary>
        /// List of all audio effects on the master track.
        /// </summary>
        public List<Effect> AudioEffects { get; set; }

        public MasterTrack(JsonNode json)
        {
            SamplingRate = json["samplingRate"].GetValue<int>();

            var loop = json["loop"];
            LoopEnabled = loop["isEnabled"].GetValue<bool>();
            LoopRange = new TimeRange(loop["begin"].GetValue<int>(), loop["end"].GetValue<int>());

            TempoTrack = new GlobalAutomationTrack(json["tempo"]);
            VolumeTrack = new AutomationTrack(json["volume"]);

            if (json["audioEffects"] != null)
                AudioEffects = Effect.FromEffectList(json["audioEffects"].AsArray());
        }
    }
}