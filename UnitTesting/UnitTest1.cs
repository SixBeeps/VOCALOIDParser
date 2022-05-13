using Microsoft.VisualStudio.TestTools.UnitTesting;
using SixBeeps.VOCALOIDParser;
using SixBeeps.VOCALOIDParser.Effects;
using System.Linq;

namespace UnitTesting
{
    [TestClass]
    public class UnitTest1
    {
        // Fill this in with the location of the DemoProjects folder
        const string BaseDir = @"C:\Users\brand\source\repos\VOCALOIDParser\DemoProjects";

        [TestMethod, TestCategory("Basic")]
        public void LoadVprTest()
        {
#pragma warning disable IDE0059 // Unnecessary assignment of a value
            var proj = VocaloidProject.CreateFromVpr(BaseDir + @"\Blank.vpr");
#pragma warning restore IDE0059 // Unnecessary assignment of a value
        }

        [TestMethod, TestCategory("Track/Part")]
        public void VocalTrackTest()
        {
            var proj = VocaloidProject.CreateFromVpr(BaseDir + @"\Vocal.vpr");
            Assert.IsTrue(proj.Tracks.First().Events.Count > 0);
        }

        [TestMethod, TestCategory("Track/Part")]
        public void LyricBuildingTest()
        {
            var proj = VocaloidProject.CreateFromVpr(BaseDir + @"\Vocal.vpr");
            var lyrics = ((VocalPart)proj.Tracks.First().Events.Values.First()).GetLyrics();
            var lyrCombined = string.Join(" ", lyrics.Values);
            Assert.AreEqual(lyrCombined, "Ooh", "The built lyrics were: " + lyrCombined);
        }

        [TestMethod, TestCategory("Track/Part")]
        public void PhonemeTest()
        {
            var proj = VocaloidProject.CreateFromVpr(BaseDir + @"\Demo.vpr");
            var glyphs = ((VocalPart)proj.Tracks.First().Events.Values.First()).Glyphs;
            var ph = (from g in glyphs where g.Value.Glyph == "the" select g).First().Value.Phonemes;
            Assert.AreEqual(ph, "D i:", "The written phoneme was: " + ph);
        }

        [TestMethod, TestCategory("Track/Part")]
        public void WaveTestTrack()
        {
            var proj = VocaloidProject.CreateFromVpr(BaseDir + @"\Wave.vpr");
            Assert.AreEqual(proj.Tracks.First().Events.Count, 2);
        }

        [TestMethod, TestCategory("Automation")]
        public void AutomationTest()
        {
            var proj = VocaloidProject.CreateFromVpr(BaseDir + @"\Automation.vpr");
            var track = proj.Tracks.First();

            // First test if automations loaded correctly
            if (track.VolumeTrack.Points.Count != 3) Assert.Fail($"Volume automation expected 3 points, got {track.VolumeTrack.Points.Count}");
            if (track.PanningTrack.Points.Count != 2) Assert.Fail($"Panning automation expected 2 points, got {track.PanningTrack.Points.Count}");

            // Then test evaluation
            float eval = track.VolumeTrack.Evaluate(TimingHelpers.BeatToTick(1));
            Assert.IsTrue(eval < 0 && eval > -898, "Evaluate call failed, value was " + eval);
        }

        [TestMethod, TestCategory("Automation")]
        public void GlobalAutomationTest()
        {
            var proj = VocaloidProject.CreateFromVpr(BaseDir + @"\Automation.vpr");
            var tempo = proj.Master.TempoTrack.GlobalValue;
            Assert.IsTrue(tempo == 12000, $"Global tempo expected 12000 hectobeats per minute, got {tempo}");
        }

        [TestMethod, TestCategory("Effects")]
        public void EffectTest()
        {
            var proj = VocaloidProject.CreateFromVpr(BaseDir + @"\ANewKindOfLove.vpr");
            Assert.IsTrue(proj.Tracks.Count > 1);
        }
    }
}