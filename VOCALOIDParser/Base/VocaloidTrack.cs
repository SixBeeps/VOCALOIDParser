using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace SixBeeps.VOCALOIDParser
{
    public class VocaloidTrack
    {
        /// <summary>
        /// Name of the track.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Whether or not this track is folded.
        /// </summary>
        public bool Folded { get; set; }

        /// <summary>
        /// List of parts or audio snippets, sorted by the time in which they are played.
        /// </summary>
        public SortedList<int, IVocaloidEvent> Events { get; set; }

        /// <summary>
        /// Volume track for this track.
        /// </summary>
        public AutomationTrack VolumeTrack { get; set; }

        /// <summary>
        /// Panning track for this track.
        /// </summary>
        public AutomationTrack PanningTrack { get; set; }

        internal int trackType;

        public VocaloidTrack() {
            Events = new();
            VolumeTrack  = new();
            PanningTrack = new();
        }

        public VocaloidTrack(JsonNode json)
        {
            Name = json["name"].ToString();
            Folded = json["isFolded"].GetValue<bool>();
            Events = new SortedList<int, IVocaloidEvent>();
            VolumeTrack = new AutomationTrack(json["volume"]);
            PanningTrack = new AutomationTrack(json["panpot"]);
        }

        internal void WriteJSON(Utf8JsonWriter jsonWriter) {
            // Base properties
            jsonWriter.WriteString("name", Name);
            jsonWriter.WriteNumber("type", trackType);
            jsonWriter.WriteBoolean("isFolded", Folded);

            // Automation tracks
            jsonWriter.WriteStartObject("volume");
            VolumeTrack.WriteJSON(jsonWriter);
            jsonWriter.WriteEndObject();
            jsonWriter.WriteStartObject("panpot");
            PanningTrack.WriteJSON(jsonWriter);
            jsonWriter.WriteEndObject();
        }
    }

    public class VocalTrack : VocaloidTrack
    {
        /// <summary>
        /// Vocal parts in this track.
        /// </summary>
        public List<VocalPart> Parts => (from part in Events select (VocalPart)part.Value).ToList();

        public VocalTrack() : base() {
            Name = "New Vocal Track";
            trackType = 0;
        }

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

    public class AudioTrack : VocaloidTrack
    {
        /// <summary>
        /// Vocal parts in this track.
        /// </summary>
        public List<AudioEvent> AudioClips => (from part in Events select (AudioEvent)part.Value).ToList();

        public AudioTrack() {
            Name = "New Audio Track";
            trackType = 1;
        }

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