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

    public class NumberHelpers
    {
        /// <summary>
        /// Linearly interpolate between two values
        /// </summary>
        /// <param name="left">The starting "base" value</param>
        /// <param name="right">The final "target" value</param>
        /// <param name="t">Range from 0.0 to 1.0 representing the percentage between the left and right values to interpolate.</param>
        /// <param name="strict">If true, throws an error if t is not within 0.0 and 1.0</param>
        /// <returns></returns>
        public static float Lerp(float left, float right, float t, bool strict = false)
        {
            if (strict)
            {
                if (t < 0.0 || t > 1.0) throw new ArgumentOutOfRangeException($"Interpolation percentage must be between 0.0 and 1.0 when strict, got {t}");
            }

            return left + (right - left) * t;
        }
    }

    public class TimeRange : IVocaloidEvent
    {
        public int StartTime { get; }
        public int EndTime { get; }
        public int Duration { get => EndTime - StartTime; }

        public TimeRange(int start, int end)
        {
            if (end < start) throw new ArgumentOutOfRangeException(nameof(end), "Attempted to create negative length TimeRange");
            StartTime = start;
            EndTime = end;
        }
    }
}
