using System.Text.Json;
using System.Text.Json.Nodes;
using SixBeeps.VOCALOIDParser.Effects;

namespace SixBeeps.VOCALOIDParser
{
    public class VocalPart : IVocaloidEvent
    {
        /// <summary>
        /// Singer for this part as a unique VOCALOID ID.
        /// </summary>
        public string SingerID { get; set; }

        /// <summary>
        /// Name of the singer for this part.
        /// </summary>
        public string SingerName => VocaloidProject.SingerNames[SingerID];

        /// <summary>
        /// Language of the singer as a unique ID.
        /// </summary>
        public int SingerLanguage { get; set; }

        /// <summary>
        /// Name of the singing style used for this part.
        /// </summary>
        public string StyleName { get; set; }

        /// <summary>
        /// Start time of this part in ticks.
        /// </summary>
        public int StartTime { get; set; }

        /// <summary>
        /// Length of this part in ticks.
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// List of all effects on this part.
        /// </summary>
        public List<Effect> AudioEffects, MidiEffects;

        /// <summary>
        /// List of sang notes ("glyphs") in the part, sorted by the time in which they are sang.
        /// </summary>
        public SortedList<int, VocalNote> Glyphs;

        public VocalPart() {
            // Grab the first known singer if it exists
            SingerID = VocaloidProject.SingerNames.Keys.FirstOrDefault("");
            SingerLanguage = 1;
            StyleName = "No Effect";
            AudioEffects = new();
            MidiEffects = new();
            Glyphs = new();
        }

        internal VocalPart(JsonNode json)
        {
            SingerID = json["voice"]["compID"].ToString();
            SingerLanguage = json["voice"]["langID"].GetValue<int>();
            StyleName = json["styleName"].ToString();
            StartTime = json["pos"].GetValue<int>();
            Duration = json["duration"].GetValue<int>();
            if (json["audioEffects"] != null)
                AudioEffects = Effect.FromEffectList(json["audioEffects"].AsArray());
            if (json["midiEffects"] != null)
                MidiEffects = Effect.FromEffectList(json["midiEffects"].AsArray());

            // Extract glyphs
            Glyphs = new SortedList<int, VocalNote>();
            int startTime, duration, midi, vel;
            string glyph, phoneme;
            DVQM atk, rel;
            JsonNode testDvqm;
            foreach (JsonNode note in json["notes"].AsArray())
            {
                startTime = note["pos"].GetValue<int>();
                duration = note["duration"].GetValue<int>();
                midi = note["number"].GetValue<int>();
                vel = note["velocity"].GetValue<int>();
                glyph = note["lyric"].ToString();
                phoneme = note["phoneme"].ToString();
                testDvqm = note["dvqm"];
                if (testDvqm != null)
                {
                    atk = testDvqm["attack"] == null ? null : new(testDvqm["attack"]);
                    rel = testDvqm["release"] == null ? null : new(testDvqm["release"]);
                }
                else atk = rel = null;
                Glyphs.Add(startTime, new VocalNote(glyph, phoneme, midi, vel, startTime, duration, atk, rel));
            }
        }

        internal void WriteJSON(Utf8JsonWriter jsonWriter) {
            // Base properties
            jsonWriter.WriteNumber("pos", StartTime);
            jsonWriter.WriteNumber("duration", Duration);
            jsonWriter.WriteString("styleName", StyleName);
            jsonWriter.WriteStartObject("voice");
            jsonWriter.WriteString("compID", SingerID);
            jsonWriter.WriteNumber("langID", SingerLanguage);
            jsonWriter.WriteEndObject();
            
            // Effects
            if (AudioEffects != null) {
                jsonWriter.WriteStartArray("audioEffects");
                foreach(var efc in AudioEffects) {
                    jsonWriter.WriteStartObject();
                    efc.WriteJSON(jsonWriter);
                    jsonWriter.WriteEndObject();
                }
                jsonWriter.WriteEndArray();
            }
            if (MidiEffects != null) {
                jsonWriter.WriteStartArray("midiEffects");
                foreach (var efc in MidiEffects) {
                    jsonWriter.WriteStartObject();
                    efc.WriteJSON(jsonWriter);
                    jsonWriter.WriteEndObject();
                }
                jsonWriter.WriteEndArray();
            }

            // Events
            jsonWriter.WriteStartArray("notes");
            foreach (var glyph in Glyphs.Values) {
                jsonWriter.WriteStartObject();
                jsonWriter.WriteNumber("pos", glyph.StartTime);
                jsonWriter.WriteNumber("duration", glyph.Duration);
                jsonWriter.WriteNumber("number", glyph.MIDINote);
                jsonWriter.WriteNumber("velocity", glyph.Velocity);
                jsonWriter.WriteString("lyric", glyph.Glyph);
                jsonWriter.WriteString("phoneme", glyph.Phonemes);
                jsonWriter.WriteStartObject("dqvm");
                if (glyph.Attack != null) {
                    jsonWriter.WriteStartObject("attack");
                    glyph.Attack.WriteJSON(jsonWriter);
                    jsonWriter.WriteEndObject();
                }
                if (glyph.Release != null) {
                    jsonWriter.WriteStartObject("release");
                    glyph.Release.WriteJSON(jsonWriter);
                    jsonWriter.WriteEndObject();
                }
                jsonWriter.WriteEndObject();
                jsonWriter.WriteEndObject();
            }
            jsonWriter.WriteEndArray();
        }

        /// <summary>
        /// Extract human-readable lyrics for this part.
        /// </summary>
        /// <returns>List of lyrics as <c>Lyric</c> instances, sorted by the time in which they are sang.</returns>
        public SortedList<int, Lyric> GetLyrics()
        {
            SortedList<int, Lyric> lyrics = new();
            List<VocalNote> wordGlyphs = new();
            foreach (VocalNote g in Glyphs.Values)
            {
                wordGlyphs.Add(g);
                if (!g.Glyph.EndsWith('-'))
                {
                    Lyric newLyric = new(wordGlyphs);
                    lyrics.Add(newLyric.StartTime, newLyric);
                    wordGlyphs.Clear();
                }
            }
            return lyrics;
        }
    }
}