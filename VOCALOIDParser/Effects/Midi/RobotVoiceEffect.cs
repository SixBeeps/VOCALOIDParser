using System.Text.Json.Nodes;

namespace SixBeeps.VOCALOIDParser.Effects
{
    public class RobotVoiceEffect : Effect
    {
        /// <summary>
        /// How fast the "autotune" effect reacts to a change in note, lower means faster.
        /// </summary>
        public float Mode { get; set; }

        public RobotVoiceEffect(JsonNode json) : base(json)
        {
            Mode = GetParamValueByName<float>("Mode");
        }
    }
}