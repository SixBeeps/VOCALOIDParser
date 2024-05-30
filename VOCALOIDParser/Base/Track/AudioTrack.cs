using System.Text.Json.Nodes;
using System.Text.Json;

namespace SixBeeps.VOCALOIDParser {
    public class AudioTrack : VocaloidTrack {
        /// <summary>
        /// Vocal parts in this track.
        /// </summary>
        public List<AudioEvent> AudioClips => (from part in Events select (AudioEvent)part.Value).ToList();

        public AudioTrack() {
            Name = "New Audio Track";
            trackType = 1;
        }

        public AudioTrack(JsonNode json) : base(json) {
            // Ignore empty tracks
            if (json["parts"] == null) return;

            string wav;
            int startTime;
            float rStart, rEnd;

            foreach (JsonNode clip in json["parts"].AsArray()) {
                wav = clip["wav"]["name"].ToString();
                startTime = clip["pos"].GetValue<int>();
                rStart = clip["region"]["begin"].GetValue<float>();
                rEnd = clip["region"]["end"].GetValue<float>();
                Events.Add(startTime, new AudioEvent(wav, startTime, rStart, rEnd));
            }
        }

        new internal void WriteJSON(Utf8JsonWriter jsonWriter) {
            base.WriteJSON(jsonWriter);
            jsonWriter.WriteStartArray("parts");
            foreach (AudioEvent clip in AudioClips) {
                jsonWriter.WriteStartObject();
                jsonWriter.WriteStartObject("wav");
                jsonWriter.WriteString("name", clip.WaveFile);
                jsonWriter.WriteEndObject();
                jsonWriter.WriteNumber("pos", clip.StartTime);
                jsonWriter.WriteStartObject("region");
                jsonWriter.WriteNumber("begin", clip.RegionStart);
                jsonWriter.WriteNumber("end", clip.RegionEnd);
                jsonWriter.WriteEndObject();
                jsonWriter.WriteEndObject();
            }
            jsonWriter.WriteEndArray();
        }
    }

}
