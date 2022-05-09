using Microsoft.VisualStudio.TestTools.UnitTesting;
using SixBeeps.VOCALOIDParser;
using System.Linq;

namespace UnitTesting
{
    [TestClass]
    public class UnitTest1
    {
        const string BaseDir = @"C:\Users\brand\source\repos\VOCALOIDParser\DemoProjects";
        [TestMethod]
        public void LoadVprTest()
        {
#pragma warning disable IDE0059 // Unnecessary assignment of a value
            var proj = VocaloidProject.CreateFromVpr(BaseDir + @"\Blank.vpr");
#pragma warning restore IDE0059 // Unnecessary assignment of a value
        }

        [TestMethod]
        public void VocalTrackTest()
        {
            var proj = VocaloidProject.CreateFromVpr(BaseDir + @"\Vocal.vpr");
            Assert.IsTrue(proj.Tracks.First().Events.Count > 0);
        }

        [TestMethod]
        public void LyricBuildingTest()
        {
            var proj = VocaloidProject.CreateFromVpr(BaseDir + @"\Vocal.vpr");
            var lyrics = (proj.Tracks.First().Events.Values.First() as VocalPart).GetLyrics();
            var lyrCombined = string.Join(" ", lyrics.Values);
            Assert.AreEqual(lyrCombined, "Ooh", "The built lyrics were: " + lyrCombined);
        }

        [TestMethod]
        public void PhonemeTest()
        {
            var proj = VocaloidProject.CreateFromVpr(BaseDir + @"\Demo.vpr");
            var glyphs = (proj.Tracks.First().Events.Values.First() as VocalPart).Glyphs;
            var ph = (from g in glyphs where g.Value.Glyph == "the" select g).First().Value.Phonemes;
            Assert.AreEqual(ph, "D i:", "The written phoneme was: " + ph);
        }

        [TestMethod]
        public void WaveTestTrack()
        {
            var proj = VocaloidProject.CreateFromVpr(BaseDir + @"\Wave.vpr");
            Assert.AreEqual(proj.Tracks.First().Events.Count, 2);
        }

        [TestMethod]
        public void AutomationTest()
        {
            var proj = VocaloidProject.CreateFromVpr(BaseDir + @"\Automation.vpr");
            var track = proj.Tracks.First();
            if (track.VolumeTrack.Points.Count != 3) Assert.Fail($"Volume automation expected 3 points, got {track.VolumeTrack.Points.Count}");
            if (track.PanningTrack.Points.Count != 2) Assert.Fail($"Panning automation expected 2 points, got {track.PanningTrack.Points.Count}");
        }
    }
}