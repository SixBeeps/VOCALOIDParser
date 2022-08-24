using System.Text.Json.Nodes;

namespace SixBeeps.VOCALOIDParser.Effects
{
    public class SingingSkillEffect : Effect
    {
        /// <summary>
        /// Name of the style, usually given as a product ID.
        /// </summary>
        public string StyleName { get; set; }

        /// <summary>
        /// How much the skill influences the notes.
        /// </summary>
        public float Amount { get; set; }

        /// <summary>
        /// How well the singer should replicate the skill.
        /// </summary>
        public float Skill { get; set; }

        public SingingSkillEffect(JsonNode json) : base(json)
        {
            Amount = GetParamValueByName<int>("Amount");
            StyleName = GetParamValueByName<string>("Name");
            Skill = GetParamValueByName<int>("Skill");
        }
    }
}