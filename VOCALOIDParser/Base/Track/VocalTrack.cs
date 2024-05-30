using System.Text.Json;
using System.Text.Json.Nodes;

namespace SixBeeps.VOCALOIDParser {
    public class VocalTrack : VocaloidTrack {
        /// <summary>
        /// Vocal parts in this track.
        /// </summary>
        public List<VocalPart> Parts => (from part in Events select (VocalPart)part.Value).ToList();

        public VocalTrack() : base() {
            Name = "New Vocal Track";
            trackType = 0;
        }

        public VocalTrack(JsonNode json) : base(json) {
            // Ignore empty tracks
            if (json["parts"] == null) return;

            int startTime;

            foreach (JsonNode part in json["parts"].AsArray()) {
                // Ignore empty parts
                if (part["notes"] == null) continue;

                startTime = part["pos"].GetValue<int>();
                Events.Add(startTime, new VocalPart(part));
            }
        }

        new internal void WriteJSON(Utf8JsonWriter jsonWriter) {
            base.WriteJSON(jsonWriter);
            jsonWriter.WriteStartArray("parts");
            foreach (VocalPart part in Parts) {
                jsonWriter.WriteStartObject();
                part.WriteJSON(jsonWriter);
                jsonWriter.WriteEndObject();
            }
            jsonWriter.WriteEndArray();
        }
    }
}
