using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace SixBeeps.VOCALOIDParser
{
    public class AutomationTrack
    {
        /// <summary>
        /// Automation points which make up the track, sorted by time in ticks.
        /// </summary>
        public SortedList<float, AutomationPoint> Points { get; private set; }

        /// <summary>
        /// Height of the automation track.
        /// </summary>
        public float Height { get; set; }

        /// <summary>
        /// Whether or not this track is folded.
        /// </summary>
        public bool Folded { get; set; }

        public AutomationTrack() {
            Points = new SortedList<float, AutomationPoint>();
            Folded = false;
            Height = 40;
        }

        public AutomationTrack(JsonNode json)
        {
            Points = new SortedList<float, AutomationPoint>();
            Folded = json["isFolded"].GetValue<bool>();
            Height = json["height"].GetValue<float>();

            float pos, val;
            foreach (JsonNode evt in json["events"].AsArray())
            {
                pos = evt["pos"].GetValue<float>();
                val = evt["value"].GetValue<float>();
                Points.Add(pos, new AutomationPoint(pos, val));
            }
        }

        /// <summary>
        /// Get the value of the automation track at a certain time.
        /// </summary>
        /// <param name="time">The time, in ticks, in which to evaluate the automation at.</param>
        /// <returns>The automation value, multiplied by 10 (234 represents 23.4 in the editor)</returns>
        public float Evaluate(int time)
        {
            // Fallback in case out of range
            if (Points.Values.Count == 0) throw new IndexOutOfRangeException("Automation has no points, cannot evaluate");
            if (time <= 0) return Points.Values.First().Value;

            // Get right-hand automation point.
            int p;
            for (p = 0; p < Points.Count; p++)
            {
                if (Points.Keys[p] >= time) break;
            }

            // If there's no right-hand point, return constant.
            if (p == Points.Count) return Points.Values[p - 1].Value;

            // Otherwise, linearly interpolate between the two.
            AutomationPoint right = right = Points.Values[p], left = Points.Values[p - 1];
            float percent = (time - left.Position) / (right.Position - left.Position);
            return NumberHelpers.Lerp(left.Value, right.Value, percent);
        }

        internal void WriteJSON(Utf8JsonWriter jsonWriter) {
            // Base properties
            jsonWriter.WriteBoolean("isFolded", Folded);
            jsonWriter.WriteNumber("height", Height);

            // Automation points
            jsonWriter.WriteStartArray("events");
            foreach (AutomationPoint point in Points.Values) {
                jsonWriter.WriteStartObject();
                jsonWriter.WriteNumber("pos", point.Value);
                jsonWriter.WriteNumber("value", point.Value);
                jsonWriter.WriteEndObject();
            }
            jsonWriter.WriteEndArray();
        }
    }

    public class GlobalAutomationTrack : AutomationTrack
    {
        /// <summary>
        /// Whether or not the global value is used.
        /// </summary>
        public bool UseGlobal { get; set; }

        /// <summary>
        /// Value used in the global context.
        /// </summary>
        public float GlobalValue { get; set; }

        public GlobalAutomationTrack() : base() {
            UseGlobal = false;
        }

        public GlobalAutomationTrack(JsonNode json) : base(json)
        {
            var gComp = json["global"];
            UseGlobal = gComp["isEnabled"].GetValue<bool>();
            GlobalValue = gComp["value"].GetValue<float>();
        }

        new internal void WriteJSON(Utf8JsonWriter jsonWriter) {
            // Global object
            jsonWriter.WriteStartObject("global");
            jsonWriter.WriteBoolean("isEnabled", UseGlobal);
            jsonWriter.WriteNumber("value", GlobalValue);
            jsonWriter.WriteEndObject();

            // Automation data
            base.WriteJSON(jsonWriter);
        }
    }

    public record struct AutomationPoint
    {
        public float Position, Value;

        public AutomationPoint(float position, float value)
        {
            Position = position;
            Value = value;
        }
    }
}