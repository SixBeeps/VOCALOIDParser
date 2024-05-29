using System.Text.Json.Nodes;

namespace SixBeeps.VOCALOIDParser.Effects
{
    public class Effect
    {
        internal JsonArray paramList;

        /// <summary>
        /// ID of the effect. This may or may not be human-readable depending on the effect.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Whether or not this effect is bypassed/disabled.
        /// </summary>
        public bool Bypassed { get; set; }

        /// <summary>
        /// Whether or not this effect is folded.
        /// </summary>
        public bool Folded { get; set; }

        /// <summary>
        /// Pre-computed list of effect UUIDs and their respective class constructors.
        /// </summary>
        protected static readonly Dictionary<string, Func<JsonNode, Effect>> EffectCreators = new() {
            { "CDB4C488-BB24-45cb-8277-A245CB228BA8", (json) => new AutoPanEffect(json) },
            { "F36904B7-5032-4cbe-AA48-C95440732F9D", (json) => new ChorusEffect(json) },
            { "4D1F311D-DA90-459d-B581-003D003A1E5E", (json) => new CompressorEffect(json) },
            { "BCE39D97-5E31-4bea-8FAB-C37BDD0FBAA4", (json) => new DeEsserEffect(json) },
            { "5AEA0D66-9D3E-4a53-B46B-925C51B94499", (json) => new DelayEffect(json) },
            { "9F301F02-A705-4b7b-9D51-E778E8338374", (json) => new DistortionEffect(json) },
            { "C7E4ED13-FF1E-4fc2-a87c-65604789469D", (json) => new EqualizerEffect(json) },
            { "C7CE3316-6FFD-4140-8a7c-0AA8B8108549", (json) => new GainEffect(json) },
            { "30F75AB7-3B14-4439-A4B8-932A63A5F31E", (json) => new PhaserEffect(json) },
            { "751EF2C0-4229-4ea7-AA0F-82EB5821D20E", (json) => new ReverbEffect(json) },
            { "3213F12A-421C-43a8-B844-7FE8C28A0D64", (json) => new TremoloEffect(json) },
            { "SingingSkill", (json) => new SingingSkillEffect(json) },
            { "VoiceColor", (json) => new VoiceColorEffect(json) },
            { "RobotVoice", (json) => new RobotVoiceEffect(json) },
            { "DefaultLyric", (json) => new DefaultLyricEffect(json) },
            { "Breath", (json) => new BreathEffect(json) }
        };

        public Effect(JsonNode json)
        {
            paramList = json["parameters"].AsArray();
            Id = json["id"].GetValue<string>();
            Bypassed = json["isBypassed"].GetValue<bool>();
            Folded = json["isFolded"].GetValue<bool>();
        }

        /// <summary>
        /// Given an array of JSON effects, return a list of <c>Effect</c> instances.
        /// </summary>
        /// <param name="json">The array to build from.</param>
        /// <returns>A <c>List<Effect></c> with the effects.</returns>
        public static List<Effect> FromEffectList(JsonArray json)
        {
            List<Effect> ret = new();
            if (json == null) return ret;
            foreach (JsonNode effect in json)
            {
                ret.Add(FromJsonEffect(effect));
            }
            return ret;
        }

        /// <summary>
        /// Routes a JSON effect to its respective effect class and returns an instances of it.
        /// </summary>
        /// <param name="json">The effect to construct.</param>
        /// <returns>A child class of <c>Effect</c> which represents the effect.</returns>
        public static Effect FromJsonEffect(JsonNode json)
        {
            if (EffectCreators.TryGetValue(json["id"].GetValue<string>(), out var createEffect)) {
                return createEffect.Invoke(json);
            }

            return new FallbackEffect(json);
        }

        protected T GetParamValueByName<T>(string name)
        {
            foreach (JsonNode parameter in paramList)
            {
                if (parameter["name"].GetValue<string>() == name)
                {
                    return parameter["value"].GetValue<T>();
                }
            }

            throw new ArgumentException($"Failed to find parameter name \"{name}\" in effect {Id}");
        }
    }
}