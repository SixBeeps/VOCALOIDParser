namespace SixBeeps.VOCALOIDParser
{
    public class TimingHelpers
    {
        public const int TICKS_PER_BEAT = 480;

        public static int BeatToTick(float time)
        {
            return (int)(time * TICKS_PER_BEAT);
        }

        public static float TickToBeat(int tick)
        {
            return (float)tick / TICKS_PER_BEAT;
        }
    }

    public class TimeRange : IVocaloidEvent
    {
        public int StartTime { get; }
        public int Duration { get; }

        public TimeRange(int start, int end)
        {
            if (end < start) throw new ArgumentOutOfRangeException(nameof(end), "Attempted to create negative length TimeRange");
            StartTime = start;
            Duration = end - start;
        }
    }
}
