#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.

using System.Text.Json.Nodes;
using VOCALOIDParser;

namespace SixBeeps.VOCALOIDParser
{
    public class VocaloidTrack
    {
        public string Name { get; }
        public bool Folded { get; set; }
        public SortedList<int, IVocaloidEvent> Events { get; }
        public AutomationTrack VolumeTrack { get; }
        public AutomationTrack PanningTrack { get; }

        public VocaloidTrack(JsonNode json)
        {
            Name = json["name"].ToString();
            Folded = json["isFolded"].GetValue<bool>();
            Events = new SortedList<int, IVocaloidEvent>();
            VolumeTrack = new AutomationTrack(json["volume"]);
            PanningTrack = new AutomationTrack(json["panpot"]);
        }
    }

    public class VocalTrack : VocaloidTrack
    {
        public VocalTrack(JsonNode json) : base(json)
        {
            // Ignore empty tracks
            if (json["parts"] == null) return;

            int startTime;

            foreach (JsonNode part in json["parts"] as JsonArray)
            {
                // Ignore empty parts
                if (part["notes"] == null) continue;

                startTime = (part["pos"] as JsonValue).GetValue<int>();
                Events.Add(startTime, new VocalPart(part));
            }
        }
    }

    public class AudioTrack : VocaloidTrack
    {
        public AudioTrack(JsonNode json) : base(json)
        {
            // Ignore empty tracks
            if (json["parts"] == null) return;

            string wav;
            int startTime;
            float rStart, rEnd;

            foreach (JsonNode clip in json["parts"] as JsonArray)
            {
                wav = clip["wav"]["name"].ToString();
                startTime = (clip["pos"] as JsonValue).GetValue<int>();
                rStart = (clip["region"]["begin"] as JsonValue).GetValue<float>();
                rEnd = (clip["region"]["end"] as JsonValue).GetValue<float>();
                Events.Add(startTime, new AudioEvent(wav, startTime, rStart, rEnd));
            }
        }
    }
}

#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.