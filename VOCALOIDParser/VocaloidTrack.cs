#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.

using System.Text.Json.Nodes;

namespace SixBeeps.VOCALOIDParser
{
    public interface IVocaloidTrack
    {
        public abstract string Name { get; }
        public abstract SortedList<int, IVocaloidEvent> Events { get; }
    }

    public class VocalTrack : IVocaloidTrack
    {
        public string Name { get; }
        public SortedList<int, IVocaloidEvent> Events { get; }

        internal VocalTrack(JsonNode json)
        {
            Name = json["name"].ToString();
            Events = new SortedList<int, IVocaloidEvent>();

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

    public class AudioTrack : IVocaloidTrack
    {
        public string Name { get; }
        public SortedList<int, IVocaloidEvent> Events { get; }

        public AudioTrack(JsonNode json)
        {
            Name = json["name"].ToString();
            Events = new SortedList<int, IVocaloidEvent>();

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

#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.