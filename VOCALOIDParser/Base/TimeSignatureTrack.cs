using System.Text.Json;
using System.Text.Json.Nodes;

namespace SixBeeps.VOCALOIDParser {
    public class TimeSignatureTrack {
        /// <summary>
        /// Time signature changes, sorted by time in bars.
        /// </summary>
        public SortedList<float, TimeSignature> Points { get; private set; }

        /// <summary>
        /// Whether or not this track is folded.
        /// </summary>
        public bool Folded { get; set; }

        public TimeSignatureTrack() {
            Points = new SortedList<float, TimeSignature>();
            Folded = false;
        }

        public TimeSignatureTrack(JsonNode json) {
            Points = new SortedList<float, TimeSignature>();
            Folded = json["isFolded"].GetValue<bool>();

            float pos;
            int numer, denom;
            foreach (JsonNode evt in json["events"].AsArray()) {
                pos = evt["bar"].GetValue<float>();
                numer = evt["numer"].GetValue<int>();
                denom = evt["denom"].GetValue<int>();
                Points.Add(pos, new TimeSignature(pos, numer, denom));
            }
        }

        public void WriteJSON(Utf8JsonWriter jsonWriter) {
            // Base properties
            jsonWriter.WriteBoolean("isFolded", Folded);

            // Time signature points
            jsonWriter.WriteStartArray("events");
            foreach (TimeSignature sig in Points.Values) {
                jsonWriter.WriteStartObject();
                jsonWriter.WriteNumber("bar", sig.Bar);
                jsonWriter.WriteNumber("numer", sig.Numerator);
                jsonWriter.WriteNumber("denom", sig.Denominator);
                jsonWriter.WriteEndObject();
            }
            jsonWriter.WriteEndArray();
        }
    }

    public record struct TimeSignature {
        public float Bar;
        public int Numerator;
        public int Denominator;

        public TimeSignature(float bar, int numerator, int denominator) {
            Bar = bar;
            Numerator = numerator;
            Denominator = denominator;
        }
    }
}
