using System.Text.Json;
using System.Text.Json.Nodes;

namespace SixBeeps.VOCALOIDParser {
    public class VocalNoteExpression {
        /// <summary>
        /// Amount of accent applied to the start of the note.
        /// </summary>
        public int Accent { get; set; } = 50;

        /// <summary>
        /// Amount of decay applied to the end of the note.
        /// </summary>
        public int Decay { get; set; } = 50;

        /// <summary>
        /// How much bending to apply to the note.
        /// </summary>
        public int BendDepth { get; set; } = 0;

        /// <summary>
        /// How long the bends last on this note.
        /// </summary>
        public int BendLength { get; set; } = 0;

        /// <summary>
        /// How open the mouth should sound.
        /// </summary>
        public int Opening { get; set; } = 127;

        public VocalNoteExpression() { }

        public VocalNoteExpression(JsonNode json) {
            Accent = json["accent"].GetValue<int>();
            Decay = json["decay"].GetValue<int>();
            BendDepth = json["bendDepth"].GetValue<int>();
            BendLength = json["bendLength"].GetValue<int>();
            Opening = json["opening"].GetValue<int>();
        }

        internal void WriteJSON(Utf8JsonWriter jsonWriter) {
            // Base properties
            jsonWriter.WriteNumber("accent", Accent);
            jsonWriter.WriteNumber("decay", Decay);
            jsonWriter.WriteNumber("bendDepth", BendDepth);
            jsonWriter.WriteNumber("bendLength", BendLength);
            jsonWriter.WriteNumber("opening", Opening);
        }
    }

    public class AIVocalNoteExpression {
        // TODO These need to be documented somehow
        public float PitchFine { get; set; } = 0.5f;
        public float PitchDriftStart { get; set; } = 0.5f;
        public float PitchDriftEnd { get; set; } = 0.5f;
        public float PitchScalingCenter { get; set; } = 0.5f;
        public float PitchScalingOrigin { get; set; } = 0.5f;
        public float PitchTransitionStart { get; set; } = 0.5f;
        public float PitchTransitionEnd { get; set; } = 0.5f;
        public float AmplitudeWhole { get; set; } = 0.5f;
        public float AmplitudeStart { get; set; } = 0.5f;
        public float AmplitudeEnd { get; set; } = 0.5f;
        public float FormantWhole { get; set; } = 0.5f;
        public float FormantStart { get; set; } = 0.5f;
        public float FormantEnd { get; set; } = 0.5f;
        public float VibratoLeadingDepth { get; set; } = 0.5f;
        public float VibratoFollowingDepth { get; set; } = 0.5f;

        public AIVocalNoteExpression(JsonNode json) {
            PitchFine = json["pitchFine"].GetValue<float>();
            PitchDriftStart = json["pitchDriftStart"].GetValue<float>();
            PitchDriftEnd = json["pitchDriftEnd"].GetValue<float>();
            PitchScalingCenter = json["pitchScalingCenter"].GetValue<float>();
            PitchScalingOrigin = json["pitchScalingOrigin"].GetValue<float>();
            PitchTransitionStart = json["pitchTransitionStart"].GetValue<float>();
            PitchTransitionEnd = json["pitchTransitionEnd"].GetValue<float>();
            AmplitudeWhole = json["amplitudeWhole"].GetValue<float>();
            AmplitudeStart = json["amplitudeStart"].GetValue<float>();
            AmplitudeEnd = json["amplitudeEnd"].GetValue<float>();
            FormantWhole = json["formantWhole"].GetValue<float>();
            FormantStart = json["formantStart"].GetValue<float>();
            FormantEnd = json["formantEnd"].GetValue<float>();
            VibratoLeadingDepth = json["vibratoLeadingDepth"].GetValue<float>();
            VibratoFollowingDepth = json["vibratoFollowingDepth"].GetValue<float>();
        }

        internal void WriteJSON(Utf8JsonWriter jsonWriter) {
            jsonWriter.WriteNumber("pitchFine", PitchFine);
            jsonWriter.WriteNumber("pitchDriftStart", PitchDriftStart);
            jsonWriter.WriteNumber("pitchDriftEnd", PitchDriftEnd);
            jsonWriter.WriteNumber("pitchScalingCenter", PitchScalingCenter);
            jsonWriter.WriteNumber("pitchScalingOrigin", PitchScalingOrigin);
            jsonWriter.WriteNumber("pitchTransitionStart", PitchTransitionStart);
            jsonWriter.WriteNumber("pitchTransitionEnd", PitchTransitionEnd);
            jsonWriter.WriteNumber("amplitudeWhole", AmplitudeWhole);
            jsonWriter.WriteNumber("amplitudeStart", AmplitudeStart);
            jsonWriter.WriteNumber("amplitudeEnd", AmplitudeEnd);
            jsonWriter.WriteNumber("formantWhole", FormantWhole);
            jsonWriter.WriteNumber("formantStart", FormantStart);
            jsonWriter.WriteNumber("formantEnd", FormantEnd);
            jsonWriter.WriteNumber("vibratoLeadingDepth", VibratoLeadingDepth);
            jsonWriter.WriteNumber("vibratoFollowingDepth", VibratoFollowingDepth);
        }
    }

    public class NoteSingingSkill {
        // TODO Also document these
        public int Duration { get; set; }
        public int PreWeight { get; set; } = 64;
        public int PostWeight { get; set; } = 64;

        public NoteSingingSkill() { }

        public NoteSingingSkill(JsonNode json) {
            Duration = json["duration"].GetValue<int>();
            PreWeight = json["weight"]["pre"].GetValue<int>();
            PostWeight = json["weight"]["post"].GetValue<int>();
        }

        internal void WriteJSON(Utf8JsonWriter jsonWriter) {
            jsonWriter.WriteNumber("duration", Duration);
            jsonWriter.WriteStartObject("weight");
            jsonWriter.WriteNumber("pre", PreWeight);
            jsonWriter.WriteNumber("post", PostWeight);
            jsonWriter.WriteEndObject();
        }
    }

    public class NoteVibrato {
        /// <summary>
        /// What kind of vibrato to apply.
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// How long to apply the vibrato for.
        /// </summary>
        public int Duration { get; set; }

        public NoteVibrato() { }

        public NoteVibrato(JsonNode json) {
            Type = json["type"].GetValue<int>();
            Duration = json["duration"].GetValue<int>();
        }

        internal void WriteJSON(Utf8JsonWriter jsonWriter) {
            jsonWriter.WriteNumber("type", Type);
            jsonWriter.WriteNumber("duration", Duration);
        }
    }
}
