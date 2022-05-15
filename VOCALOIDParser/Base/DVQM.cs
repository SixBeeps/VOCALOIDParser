using System.Text.Json.Nodes;

namespace SixBeeps.VOCALOIDParser
{
    public class DVQM
    {
        /// <summary>
        /// ID of the attack/release effect.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// List of human-readable names for the attack/release effect.
        /// </summary>
        public string[] FriendlyNames { get; set; }

        /// <summary>
        /// Whether or not this attack/release effect is protected or not.
        /// </summary>
        public bool Protected { get; set; }

        /// <summary>
        /// Speed of the attack/release effect.
        /// </summary>
        public int Speed { get; set; }

        /// <summary>
        /// Harshness of the attack/release effect.
        /// </summary>
        public float TopFactor { get; set; }

        public DVQM(JsonNode json)
        {
            Id = json["compID"].GetValue<string>();
            FriendlyNames = (from name in json["levelNames"].AsArray() select name.GetValue<string>()).ToArray();
            Protected = json["isProtected"].GetValue<bool>();
            Speed = json["speed"].GetValue<int>();
            TopFactor = json["topFactor"].GetValue<float>();
        }
    }
}
