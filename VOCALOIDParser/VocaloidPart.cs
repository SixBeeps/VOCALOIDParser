﻿#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.

using System.Text.Json.Nodes;
using SixBeeps.VOCALOIDParser.Effects;

namespace SixBeeps.VOCALOIDParser
{
    public class VocalPart : IVocaloidEvent
    {
        /// <summary>
        /// Singer for this part as a unique VOCALOID ID.
        /// </summary>
        public string SingerID { get; }

        /// <summary>
        /// Name of the singer for this part.
        /// </summary>
        public string SingerName => VocaloidProject.SingerNames[SingerID];

        /// <summary>
        /// Start time of this part in ticks.
        /// </summary>
        public int StartTime { get; }

        /// <summary>
        /// Length of this part in ticks.
        /// </summary>
        public int Duration { get; }

        /// <summary>
        /// List of all effects on this part.
        /// </summary>
        public List<Effect> AudioEffects, MidiEffects;

        /// <summary>
        /// List of sang notes ("glyphs") in the part, sorted by the time in which they are sang.
        /// </summary>
        public SortedList<int, VocalNote> Glyphs;

        internal VocalPart(JsonNode json)
        {
            SingerID = json["voice"]["compID"].ToString();
            StartTime = json["pos"].GetValue<int>();
            Duration = json["duration"].GetValue<int>();
            if (json["audioEffects"] != null)
                AudioEffects = Effect.FromEffectList(json["audioEffects"].AsArray());
            if (json["midiEffects"] != null)
                MidiEffects = Effect.FromEffectList(json["midiEffects"].AsArray());

            // Extract glyphs
            Glyphs = new SortedList<int, VocalNote>();
            int startTime, duration, midi;
            string glyph, phoneme;
            foreach (JsonNode note in json["notes"].AsArray())
            {
                startTime = note["pos"].GetValue<int>();
                duration = note["duration"].GetValue<int>();
                midi = note["number"].GetValue<int>();
                glyph = note["lyric"].ToString();
                phoneme = note["phoneme"].ToString();
                Glyphs.Add(startTime, new VocalNote(glyph, phoneme, midi, startTime, duration));
            }
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

#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.