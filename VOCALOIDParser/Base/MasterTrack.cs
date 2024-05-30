using System.Text.Json;
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

        public MasterTrack() {
            SamplingRate = 44100;
            LoopEnabled = false;
            TempoTrack = new GlobalAutomationTrack();
            VolumeTrack = new AutomationTrack();
            AudioEffects = new List<Effect>();
        }

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
            else
                AudioEffects = new List<Effect>();
        }

        internal void WriteJSON(Utf8JsonWriter jsonWriter) {
            // Base properties
            jsonWriter.WriteNumber("samplingRate", SamplingRate);

            // Loop
            jsonWriter.WriteStartObject("loop");
            jsonWriter.WriteBoolean("isEnabled", LoopEnabled);
            if (LoopRange != null) {
                jsonWriter.WriteNumber("begin", LoopRange.StartTime);
                jsonWriter.WriteNumber("end", LoopRange.EndTime);
            }
            jsonWriter.WriteEndObject();

            // Automation tracks
            jsonWriter.WriteStartObject("tempo");
            TempoTrack.WriteJSON(jsonWriter);
            jsonWriter.WriteEndObject();

            jsonWriter.WriteStartObject("volume");
            VolumeTrack.WriteJSON(jsonWriter);
            jsonWriter.WriteEndObject();

            // Audio effects
            jsonWriter.WriteStartArray("audioEffects");
            foreach (Effect effect in AudioEffects) {
                effect.WriteJSON(jsonWriter);
            }
            jsonWriter.WriteEndArray();
        }
    }
}