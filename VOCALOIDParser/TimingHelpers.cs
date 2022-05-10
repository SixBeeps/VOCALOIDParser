using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
