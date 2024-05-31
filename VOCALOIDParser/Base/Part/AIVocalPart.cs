using System.Text.Json;
using System.Text.Json.Nodes;
using SixBeeps.VOCALOIDParser.Effects;

namespace SixBeeps.VOCALOIDParser
{
    public class AIVocalPart : IVocaloidEvent
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
        /// Languages of the singer as unique IDs.
        /// </summary>
        public int[] SingerLanguages { get; set; }

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
        public SortedList<int, AIVocalNote> Glyphs;

        public AIVocalPart() {
            // Grab the first known singer if it exists
            SingerID = VocaloidProject.SingerNames.Keys.FirstOrDefault("");
            SingerLanguages = [0];
            StyleName = "No Effect";
            AudioEffects = new();
            MidiEffects = new();
            Glyphs = new();
        }

        internal AIVocalPart(JsonNode json)
        {
            SingerID = json["aiVoice"]["compID"].ToString();
            SingerLanguages = json["aiVoice"]["langIDs"].AsArray().Select(x => x["langID"].GetValue<int>()).ToArray();
            StyleName = json["styleName"].ToString();
            StartTime = json["pos"].GetValue<int>();
            Duration = json["duration"].GetValue<int>();
            if (json["audioEffects"] != null)
                AudioEffects = Effect.FromEffectList(json["audioEffects"].AsArray());
            if (json["midiEffects"] != null)
                MidiEffects = Effect.FromEffectList(json["midiEffects"].AsArray());

            // Extract glyphs
            Glyphs = new SortedList<int, AIVocalNote>();
            if (json["notes"] == null) return;
            int startTime, duration, midi, vel, langId;
            string glyph, phoneme;
            VocalNoteExpression exp;
            AIVocalNoteExpression aiExp;
            NoteSingingSkill skill;
            NoteVibrato vib;
            foreach (JsonNode note in json["notes"].AsArray())
            {
                startTime = note["pos"].GetValue<int>();
                duration = note["duration"].GetValue<int>();
                midi = note["number"].GetValue<int>();
                vel = note["velocity"].GetValue<int>();
                glyph = note["lyric"].ToString();
                phoneme = note["phoneme"].ToString();
                langId = note["langID"].GetValue<int>();
                exp = new(note["exp"]);
                aiExp = new(note["aiExp"]);
                skill = new(note["singingSkill"]);
                vib = new(note["vibrato"]);
                Glyphs.Add(startTime, new AIVocalNote(glyph, phoneme, langId, midi, vel, startTime, duration, exp, aiExp, skill, vib));
            }
        }

        internal void WriteJSON(Utf8JsonWriter jsonWriter) {
            // Base properties
            jsonWriter.WriteNumber("pos", StartTime);
            jsonWriter.WriteNumber("duration", Duration);
            jsonWriter.WriteString("styleName", StyleName);
            jsonWriter.WriteStartObject("voice");
            jsonWriter.WriteString("compID", SingerID);
            jsonWriter.WriteStartArray("langIDs");
            foreach (int lang in SingerLanguages) {
                jsonWriter.WriteStartObject();
                jsonWriter.WriteNumber("langID", lang);
                jsonWriter.WriteEndObject();
            }
            jsonWriter.WriteEndArray();
            jsonWriter.WriteEndObject();
            
            // Effects
            jsonWriter.WriteStartArray("audioEffects");
            foreach(var efc in AudioEffects) {
                jsonWriter.WriteStartObject();
                efc.WriteJSON(jsonWriter);
                jsonWriter.WriteEndObject();
            }
            jsonWriter.WriteEndArray();
            jsonWriter.WriteStartArray("midiEffects");
            foreach (var efc in MidiEffects) {
                jsonWriter.WriteStartObject();
                efc.WriteJSON(jsonWriter);
                jsonWriter.WriteEndObject();
            }
            jsonWriter.WriteEndArray();

            // Events
            jsonWriter.WriteStartArray("notes");
            foreach (var glyph in Glyphs.Values) {
                jsonWriter.WriteStartObject();
                jsonWriter.WriteString("lyric", glyph.Glyph);
                jsonWriter.WriteString("phoneme", glyph.Phonemes);
                jsonWriter.WriteNumber("pos", glyph.StartTime);
                jsonWriter.WriteNumber("duration", glyph.Duration);
                jsonWriter.WriteNumber("number", glyph.MIDINote);
                jsonWriter.WriteNumber("velocity", glyph.Velocity);
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
            List<AIVocalNote> wordGlyphs = new();
            foreach (AIVocalNote g in Glyphs.Values)
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