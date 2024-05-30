using System.Text.Json;
using System.Text.Json.Nodes;

namespace SixBeeps.VOCALOIDParser {
    public class AIVocalTrack : VocaloidTrack {
        /// <summary>
        /// Vocal parts in this track.
        /// </summary>
        public List<AIVocalPart> Parts => (from part in Events select (AIVocalPart)part.Value).ToList();

        public AIVocalTrack() : base() {
            Name = "New AI Vocal Track";
            trackType = 2;
        }

        public AIVocalTrack(JsonNode json) : base(json) {
            // Ignore empty tracks
            if (json["parts"] == null) return;

            int startTime;

            foreach (JsonNode part in json["parts"].AsArray()) {
                // Ignore empty parts
                if (part["notes"] == null) continue;

                startTime = part["pos"].GetValue<int>();
                Events.Add(startTime, new AIVocalPart(part));
            }
        }

        new internal void WriteJSON(Utf8JsonWriter jsonWriter) {
            base.WriteJSON(jsonWriter);
            jsonWriter.WriteStartArray("parts");
            foreach (AIVocalPart part in Parts) {
                jsonWriter.WriteStartObject();
                part.WriteJSON(jsonWriter);
                jsonWriter.WriteEndObject();
            }
            jsonWriter.WriteEndArray();
        }
    }
}
