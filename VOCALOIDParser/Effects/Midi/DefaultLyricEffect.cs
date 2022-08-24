using System.Text.Json.Nodes;

namespace SixBeeps.VOCALOIDParser.Effects
{
    public class DefaultLyricEffect : Effect
    {
        /// <summary>
        /// Default lyrics to use for each language
        /// </summary>
        public Dictionary<string, string> Lyrics { get; set; }

        public DefaultLyricEffect(JsonNode json) : base(json)
        {
            Lyrics = new Dictionary<string, string>();
            foreach (JsonNode lang in paramList)
            {
                Lyrics.Add(lang["name"].GetValue<string>(), lang["value"].GetValue<string>());
            }
        }
    }
}