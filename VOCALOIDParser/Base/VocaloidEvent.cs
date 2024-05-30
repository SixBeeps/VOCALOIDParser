using System.Linq;
using System.Text.Json;

namespace SixBeeps.VOCALOIDParser
{
    public interface IVocaloidEvent
    {
        int StartTime { get; set; }
        int Duration { get; set; }
    }

    public record struct VocalNote : IVocaloidEvent
    {
        public string Glyph;
        public string Phonemes;
        public int MIDINote;
        public int Velocity;
        public int StartTime { get; set; }
        public int Duration { get; set; }
        public DVQM Attack, Release;

        public VocalNote(string glyph, string phonemes, int midi, int vel, int startTime, int duration, DVQM atk, DVQM rel)
        {
            Glyph = glyph;
            Phonemes = phonemes;
            MIDINote = midi;
            Velocity = vel;
            StartTime = startTime;
            Duration = duration;
            Attack = atk;
            Release = rel;
        }
    }

    public struct Lyric : IVocaloidEvent
    {
        public string Word;
        public string Phonemes;
        public int StartTime { get; set; }
        public int Duration { get; set; }

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
        public int StartTime { get; set; }
        public int Duration {
            get => (int)(RegionEnd - RegionStart);
            set => RegionEnd = RegionStart + value;
        }
        public float RegionStart { get; set; }
        public float RegionEnd { get; set; }

        public AudioEvent(string wavFile, int startTime, float regionStart, float regionEnd)
        {
            WaveFile = wavFile;
            StartTime = startTime;
            RegionStart = regionStart;
            RegionEnd = regionEnd;
        }
    }
}
