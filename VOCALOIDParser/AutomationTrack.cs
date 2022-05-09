#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
using System.Collections.Generic;
using System.Text.Json.Nodes;

namespace VOCALOIDParser
{
    public class AutomationTrack
    {
        public List<AutomationPoint> Points { get; private set; }
        public bool Folded { get; set; }

        public AutomationTrack(JsonNode json)
        {
            Points = new List<AutomationPoint>();
            Folded = json["isFolded"].GetValue<bool>();

            float pos, val;
            foreach (JsonNode evt in json["events"] as JsonArray)
            {
                pos = (evt["pos"] as JsonValue).GetValue<float>();
                val = (evt["value"] as JsonValue).GetValue<float>();
                Points.Add(new AutomationPoint(pos, val));
            }
        }

        public float Evaluate(float time)
        {
            throw new NotImplementedException();
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