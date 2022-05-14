using System.Linq;

namespace SixBeeps.VOCALOIDParser
{
    public interface IVocaloidEvent
    {
        int StartTime { get; }
        int Duration { get; }
    }

    public record struct VocalNote : IVocaloidEvent
    {
        public string Glyph;
        public string Phonemes;
        public int MIDINote;
        public int StartTime { get; }
        public int Duration { get; }

        public VocalNote(string glyph, string phonemes, int midi, int startTime, int duration)
        {
            Glyph = glyph;
            Phonemes = phonemes;
            MIDINote = midi;
            StartTime = startTime;
            Duration = duration;
        }
    }

    public struct Lyric : IVocaloidEvent
    {
        public string Word;
        public string Phonemes;
        public int StartTime { get; }
        public int Duration { get; }

        public Lyric(List<VocalNote> notes)
        {
            if (notes.Count == 0) throw new ArgumentOutOfRangeException(nameof(notes), "Empty notes list given");

            Word = string.Empty;
            Phonemes = string.Empty;
            StartTime = notes.First().StartTime;
            Duration = 0;
            foreach (VocalNote note in notes)
            {
                Word += note.Glyph.Replace("-", "");
                Phonemes += note.Phonemes;
                Duration += note.Duration;
            }
        }

        public override string ToString()
        {
            return Word;
        }
    }

    public record struct AudioEvent : IVocaloidEvent
    {
        public string WaveFile;
        public int StartTime { get; }
        public int Duration => (int)(RegionEnd - RegionStart);
        public float RegionStart { get; }
        public float RegionEnd { get; }

        public AudioEvent(string wavFile, int startTime, float regionStart, float regionEnd)
        {
            WaveFile = wavFile;
            StartTime = startTime;
            RegionStart = regionStart;
            RegionEnd = regionEnd;
        }
    }
}
