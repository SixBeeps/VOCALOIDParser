#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.

using System.Text.Json.Nodes;

namespace SixBeeps.VOCALOIDParser
{
    public class VocaloidTrack
    {
        /// <summary>
        /// Name of the track.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Whether or not this track is folded.
        /// </summary>
        public bool Folded { get; set; }

        /// <summary>
        /// List of parts or audio snippets, sorted by the time in which they are played.
        /// </summary>
        public SortedList<int, IVocaloidEvent> Events { get; }

        /// <summary>
        /// Volume track for this track.
        /// </summary>
        public AutomationTrack VolumeTrack { get; }

        /// <summary>
        /// Panning track for this track.
        /// </summary>
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
        /// <summary>
        /// Vocal parts in this track.
        /// </summary>
        public List<VocalPart> Parts => (from part in Events select (VocalPart)part.Value).ToList();

        public VocalTrack(JsonNode json) : base(json)
        {
            // Ignore empty tracks
            if (json["parts"] == null) return;

            int startTime;

            foreach (JsonNode part in json["parts"].AsArray())
            {
                // Ignore empty parts
                if (part["notes"] == null) continue;

                startTime = part["pos"].GetValue<int>();
                Events.Add(startTime, new VocalPart(part));
            }
        }
    }

    public class AudioTrack : VocaloidTrack
    {
        /// <summary>
        /// Vocal parts in this track.
        /// </summary>
        public List<AudioEvent> AudioClips => (from part in Events select (AudioEvent)part.Value).ToList();

        public AudioTrack(JsonNode json) : base(json)
        {
            // Ignore empty tracks
            if (json["parts"] == null) return;

            string wav;
            int startTime;
            float rStart, rEnd;

            foreach (JsonNode clip in json["parts"].AsArray())
            {
                wav = clip["wav"]["name"].ToString();
                startTime = clip["pos"].GetValue<int>();
                rStart = clip["region"]["begin"].GetValue<float>();
                rEnd = clip["region"]["end"].GetValue<float>();
                Events.Add(startTime, new AudioEvent(wav, startTime, rStart, rEnd));
            }
        }
    }
}

#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.