namespace SixBeeps.VOCALOIDParser
{
    public class TimingHelpers
    {
        /// <summary>
        /// How many ticks are in a quarter note. This is specific to VOCALOID5, and should be the same across clients.
        /// </summary>
        public const int TICKS_PER_BEAT = 480;

        /// <summary>
        /// Converts quarter note beats to ticks
        /// </summary>
        /// <param name="time">The quarter note position</param>
        /// <returns>The tick position</returns>
        public static int BeatToTick(float time)
        {
            return (int)(time * TICKS_PER_BEAT);
        }

        /// <summary>
        /// Converts ticks to quarter note beats
        /// </summary>
        /// <param name="tick">The tick position</param>
        /// <returns>The quarter note position</returns>
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
