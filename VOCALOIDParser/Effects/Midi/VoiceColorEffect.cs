#pragma warning disable CS8602 // Dereference of a possibly null reference.
using System.Text.Json.Nodes;

namespace SixBeeps.VOCALOIDParser.Effects
{
    public class VoiceColorEffect : Effect
    {
        /// <summary>
        /// How much air is let out when singing, typically smoother than breath.
        /// </summary>
        public int Air { get; set; }

        /// <summary>
        /// How much breath is let out when singing, typically sharper than air.
        /// </summary>
        public int Breathiness { get; set; }

        /// <summary>
        /// How cute/cool the singer sounds.
        /// </summary>
        public int Character { get; set; }

        /// <summary>
        /// General warmth/brightness of the voice.
        /// </summary>
        public int Excitement { get; set; }

        /// <summary>
        /// How fried the voice is.
        /// </summary>
        public int Growl { get; set; }

        /// <summary>
        /// How open the mouth of the singer is.
        /// </summary>
        public int Mouth { get; set; }

        public VoiceColorEffect(JsonNode json) : base(json)
        {
            Air = GetParamValueByName<int>("Air");
            Breathiness = GetParamValueByName<int>("Breathiness");
            Character = GetParamValueByName<int>("Character");
            Excitement = GetParamValueByName<int>("Exciter");
            Growl = GetParamValueByName<int>("Growl");
            Mouth = GetParamValueByName<int>("Mouth");
        }
    }
}
#pragma warning restore CS8602 // Dereference of a possibly null reference.