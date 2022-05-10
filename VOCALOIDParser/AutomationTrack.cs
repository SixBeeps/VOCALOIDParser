#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
using System.Collections.Generic;
using System.Text.Json.Nodes;

namespace SixBeeps.VOCALOIDParser
{
    public class AutomationTrack
    {
        public SortedList<float, AutomationPoint> Points { get; private set; }
        public bool Folded { get; set; }

        public AutomationTrack(JsonNode json)
        {
            Points = new SortedList<float, AutomationPoint>();
            Folded = json["isFolded"].GetValue<bool>();

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
            if (time <= 0) return Points.Values[0].Value;

            // Get right-hand automation point.
            int p;
            for (p = 0; p < Points.Count; p++)
            {
                if (Points.Keys[p] >= time) break;
            }

            // If there's no right-hand point, return constant.
            if (p == Points.Count - 1) return Points.Values[p].Value;

            // Otherwise, linearly interpolate between the two.
            AutomationPoint left = Points.Values[p - 1], right = Points.Values[p];
            float percent = (time - left.Position) / (right.Position - left.Position);
            float del = (right.Value - left.Value) * percent;
            return Points.Values[p - 1].Value + del;
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

#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.