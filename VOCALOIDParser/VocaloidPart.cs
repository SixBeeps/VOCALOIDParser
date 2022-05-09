#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.

using System.Text.Json.Nodes;

namespace SixBeeps.VOCALOIDParser
{
    public class VocalPart : IVocaloidEvent
    {
        public string SingerID { get; }
        public string SingerName => VocaloidProject.SingerNames[SingerID];

        public int StartTime { get; }

        public int Duration { get; }

        public SortedList<int, VocalNote> Glyphs;

        internal VocalPart(JsonNode json)
        {
            SingerID = json["voice"]["compID"].ToString();
            Glyphs = new SortedList<int, VocalNote>();

            StartTime = json["pos"].GetValue<int>();
            Duration = json["duration"].GetValue<int>();

            // Extract glyphs
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