#pragma warning disable CS8602 // Dereference of a possibly null reference.
using System.Text.Json.Nodes;

namespace SixBeeps.VOCALOIDParser.Effects
{
    public class EqualizerEffect : Effect
    {
        /// <summary>
        /// Whether or not a highpass filter is applied.
        /// </summary>
        public float HPF { get; set; }

        /// <summary>
        /// Frequency band of the EQ effect.
        /// </summary>
        public EQBand Low, LowMid, HighMid, High;

        public EqualizerEffect(JsonNode json) : base(json)
        {
            HPF = GetParamValueByName<float>("HPF");

            // Low and high bands do not have Q values.
            Low = new EQBand(
                GetParamValueByName<float>("Low F"),
                GetParamValueByName<float>("Low G"),
                -1f
            );
            High = new EQBand(
                GetParamValueByName<float>("High F"),
                GetParamValueByName<float>("High G"),
                -1f
            );

            // Low-mid and high-mid bands, however, DO have them.
            LowMid = new EQBand(
                GetParamValueByName<float>("Low Mid F"),
                GetParamValueByName<float>("Low Mid G"),
                GetParamValueByName<float>("Low Mid Q")
            );
            HighMid = new EQBand(
                GetParamValueByName<float>("High Mid F"),
                GetParamValueByName<float>("High Mid G"),
                GetParamValueByName<float>("High Mid Q")
            );
        }
    }

    public struct EQBand
    {
        private float qStored;
        /// <summary>
        /// Frequency of the EQ band.
        /// </summary>
        public float Frequency { get; set; }

        /// <summary>
        /// Gain of the EQ band.
        /// </summary>
        public float Gain { get; set; }

        /// <summary>
        /// Quality value ("thinness") of the EQ band.
        /// </summary>
        public float Q {
            get {
                if (qStored < 0f) throw new InvalidOperationException("This EQ band does not have a Q value.");
                return qStored;
            }
            set => qStored = value;
        }

        public EQBand(float freq, float gain, float q)
        {
            Frequency = freq;
            Gain = gain;
            qStored = q;
        }
    }
}
#pragma warning restore CS8602 // Dereference of a possibly null reference.