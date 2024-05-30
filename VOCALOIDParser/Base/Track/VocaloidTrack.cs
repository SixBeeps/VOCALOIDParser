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
        /// Which bus this track is routed to. Defaults to 0 on VOCALOID5.
        /// </summary>
        public int Bus { get; set; }

        /// <summary>
        /// Height of the track in the editor.
        /// </summary>
        public float Height { get; set; }

        /// <summary>
        /// Color of the track in the editor.
        /// </summary>
        public int Color { get; set; }

        /// <summary>
        /// Whether or not this track is muted.
        /// </summary>
        public bool Muted { get; set; }

        /// <summary>
        /// Whether or not this track is soloed.
        /// </summary>
        public bool Soloed { get; set; }

        /// <summary>
        /// Scroll position of the piano roll as a MIDI note number.
        /// </summary>
        public int LastScrollPosition { get; set; }

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

        public VocaloidTrack()
        {
            Height = 80;
            LastScrollPosition = 64;
            Events = new();
            VolumeTrack = new();
            PanningTrack = new();
        }

        public VocaloidTrack(JsonNode json)
        {
            Name = json["name"].ToString();
            Bus = json["busNo"].GetValue<int>();
            Height = json["height"].GetValue<float>();
            Folded = json["isFolded"].GetValue<bool>();
            Muted = json["isMuted"].GetValue<bool>();
            Soloed = json["isSoloMode"].GetValue<bool>();
            LastScrollPosition = json["lastScrollPositionNoteNumber"]?.GetValue<int>() ?? 64;
            Events = new SortedList<int, IVocaloidEvent>();
            VolumeTrack = new AutomationTrack(json["volume"]);
            PanningTrack = new AutomationTrack(json["panpot"]);
        }

        internal void WriteJSON(Utf8JsonWriter jsonWriter)
        {
            // Base properties
            jsonWriter.WriteNumber("type", trackType);
            jsonWriter.WriteString("name", Name);
            jsonWriter.WriteNumber("color", Color);
            jsonWriter.WriteNumber("busNo", Bus);
            jsonWriter.WriteBoolean("isFolded", Folded);
            jsonWriter.WriteNumber("height", Height);
            jsonWriter.WriteBoolean("isMuted", Muted);
            jsonWriter.WriteBoolean("isSoloMode", Soloed);
            jsonWriter.WriteNumber("lastScrollPositionNoteNumber", LastScrollPosition);

            // Automation tracks
            jsonWriter.WriteStartObject("volume");
            VolumeTrack.WriteJSON(jsonWriter);
            jsonWriter.WriteEndObject();
            jsonWriter.WriteStartObject("panpot");
            PanningTrack.WriteJSON(jsonWriter);
            jsonWriter.WriteEndObject();
        }
    }
}