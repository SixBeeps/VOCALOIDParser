using System.Text.Json.Nodes;

namespace SixBeeps.VOCALOIDParser
{
    public class MasterTrack
    {
        public int SamplingRate { get; set; }
        public bool LoopEnabled { get; set; }
        public TimeRange LoopRange { get; set; }
        public GlobalAutomationTrack TempoTrack { get; set; }
        public AutomationTrack VolumeTrack { get; set; }

        public MasterTrack(JsonNode json)
        {
            SamplingRate = json["samplingRate"].GetValue<int>();

            var loop = json["loop"];
            LoopEnabled = loop["isEnabled"].GetValue<bool>();
            LoopRange = new TimeRange(loop["begin"].GetValue<int>(), loop["end"].GetValue<int>());
            TempoTrack = new GlobalAutomationTrack(json["tempo"]);
            VolumeTrack = new AutomationTrack(json["volume"]);
        }
    }
}
