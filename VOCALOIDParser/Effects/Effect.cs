#pragma warning disable CS8602 // Dereference of a possibly null reference.
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

        public Effect(JsonNode json)
        {
            paramList = json["Parameters"].AsArray();
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
            switch (json["id"].GetValue<string>())
            {
                case "CDB4C488-BB24-45cb-8277-A245CB228BA8":
                    return new AutoPanEffect(json);
                case "F36904B7-5032-4cbe-AA48-C95440732F9D":
                    return new ChorusEffect(json);
                case "4D1F311D-DA90-459d-B581-003D003A1E5E":
                    return new CompressorEffect(json);
                case "BCE39D97-5E31-4bea-8FAB-C37BDD0FBAA4":
                    return new DeEsserEffect(json);
                case "5AEA0D66-9D3E-4a53-B46B-925C51B94499":
                    return new DelayEffect(json);
                case "9F301F02-A705-4b7b-9D51-E778E8338374":
                    return new DistortionEffect(json);
                case "C7E4ED13-FF1E-4fc2-a87c-65604789469D":
                    return new EqualizerEffect(json);
                case "C7CE3316-6FFD-4140-8a7c-0AA8B8108549":
                    return new GainEffect(json);
                case "30F75AB7-3B14-4439-A4B8-932A63A5F31E":
                    return new PhaserEffect(json);
                case "751EF2C0-4229-4ea7-AA0F-82EB5821D20E":
                    return new ReverbEffect(json);
                case "3213F12A-421C-43a8-B844-7FE8C28A0D64":
                    return new TremoloEffect(json);

                default:
                    return null;
            }
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
#pragma warning restore CS8602 // Dereference of a possibly null reference.