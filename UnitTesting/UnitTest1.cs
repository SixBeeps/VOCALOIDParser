using Microsoft.VisualStudio.TestTools.UnitTesting;
using SixBeeps.VOCALOIDParser;
using System.Linq;

namespace UnitTesting
{
    [TestClass]
    public class UnitTest1
    {
        const string BaseDir = @"D:\VOCALOID5\Documents\Testing";
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
    }
}