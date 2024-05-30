using Microsoft.VisualStudio.TestTools.UnitTesting;
using SixBeeps.VOCALOIDParser;
using SixBeeps.VOCALOIDParser.Effects;
using System.IO;
using System.Linq;

namespace UnitTesting
{
    [TestClass]
    public class UnitTest1
    {
        // Fill this in with the location of the DemoProjects folder
        //const string BASE_DIR = @"C:\Users\brand\source\repos\VOCALOIDParser\DemoProjects";
        string BASE_DIR = Path.Join(
            Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
            "DemoProjects"
        );

        [TestMethod, TestCategory("Basic")]
        public void LoadVprTest()
        {
            var proj = VocaloidProject.CreateFromVpr(BASE_DIR + @"\Blank.vpr");
        }

        [TestMethod, TestCategory("Track/Part")]
        public void VocalTrackTest()
        {
            var proj = VocaloidProject.CreateFromVpr(BASE_DIR + @"\Vocal.vpr");
            Assert.IsTrue(proj.Tracks.First().Events.Count > 0);
        }

        [TestMethod, TestCategory("Track/Part")]
        public void WaveTestTrack()
        {
            var proj = VocaloidProject.CreateFromVpr(BASE_DIR + @"\Wave.vpr");
            Assert.AreEqual(proj.Tracks.First().Events.Count, 2);
        }
        
        [TestMethod, TestCategory("Track/Part")]
        public void LyricBuildingTest()
        {
            var proj = VocaloidProject.CreateFromVpr(BASE_DIR + @"\Vocal.vpr");
            var lyrics = ((VocalPart)proj.Tracks.First().Events.Values.First()).GetLyrics();
            var lyrCombined = string.Join(" ", lyrics.Values);
            Assert.AreEqual(lyrCombined, "Ooh", "The built lyrics were: " + lyrCombined);
        }

        [TestMethod, TestCategory("Track/Part")]
        public void PhonemeTest()
        {
            var proj = VocaloidProject.CreateFromVpr(BASE_DIR + @"\Demo.vpr");
            var glyphs = ((VocalPart)proj.Tracks.First().Events.Values.First()).Glyphs;
            var ph = (from g in glyphs where g.Value.Glyph == "the" select g).First().Value.Phonemes;
            Assert.AreEqual(ph, "D i:", "The written phoneme was: " + ph);
        }

        [TestMethod, TestCategory("Track/Part")]
        public void DVWMTest()
        {
            var proj = VocaloidProject.CreateFromVpr(BASE_DIR + @"\ANewKindOfLove.vpr");
            Assert.IsTrue(((VocalPart)proj.Tracks.First().Events.First().Value).Glyphs.ElementAt(2).Value.Attack.FriendlyNames.Contains("Up2"));
        }

        [TestMethod, TestCategory("Automation")]
        public void AutomationTest()
        {
            var proj = VocaloidProject.CreateFromVpr(BASE_DIR + @"\Automation.vpr");
            var track = proj.Tracks.First();

            // First test if automations loaded correctly
            if (track.VolumeTrack.Points.Count != 3) Assert.Fail($"Volume automation expected 3 points, got {track.VolumeTrack.Points.Count}");
            if (track.PanningTrack.Points.Count != 2) Assert.Fail($"Panning automation expected 2 points, got {track.PanningTrack.Points.Count}");

            // Then test evaluation
            float eval = track.VolumeTrack.Evaluate(TimingHelpers.BeatToTick(1));
            Assert.IsTrue(eval < 0 && eval > -898, "Evaluate call failed, value was " + eval);
        }

        [TestMethod, TestCategory("Automation")]
        public void AutomationNoEventsTest()
        {
            var proj = VocaloidProject.CreateFromVpr(BASE_DIR + @".\Automation.vpr");

            // Test if tempo automation matches project tempo
            float eval = proj.Master.TempoTrack.Evaluate(TimingHelpers.BeatToTick(2)) / 100;
            Assert.IsTrue(eval == proj.BPM, $"Evaluated {eval}, real BPM {proj.BPM}");
        }

        [TestMethod, TestCategory("Automation")]
        public void GlobalAutomationTest()
        {
            var proj = VocaloidProject.CreateFromVpr(BASE_DIR + @"\Automation.vpr");
            var tempo = proj.Master.TempoTrack.GlobalValue;
            Assert.IsTrue(tempo == 12000, $"Global tempo expected 12000 hectobeats per minute, got {tempo}");
        }

        [TestMethod, TestCategory("Effects")]
        public void EffectTest()
        {
            var proj = VocaloidProject.CreateFromVpr(BASE_DIR + @"\ANewKindOfLove.vpr");
            var firstPart = (VocalPart)proj.Tracks.First().Events.First().Value;
            var firstEffect = firstPart.AudioEffects.First();
            if (firstEffect is not ChorusEffect)
                Assert.Fail($"Expected first audio effect to be Chorus, got {firstEffect.GetType().Name}");
            firstEffect = firstPart.MidiEffects.First();
            if (firstEffect is not SingingSkillEffect)
                Assert.Fail($"Expected first MIDI effect to be SingingSkill, got {firstEffect.GetType().Name}");
        }

        [TestMethod, TestCategory("Effects")]
        public void GlobalEffectTest()
        {
            var proj = VocaloidProject.CreateFromVpr(BASE_DIR + @"\ANewKindOfLove.vpr");
            var firstEffect = proj.Master.AudioEffects.First();
            if (firstEffect is not ReverbEffect)
                Assert.Fail($"Expected first audio effect to be Reverb, got {firstEffect.GetType().Name}");
        }

        [TestMethod, TestCategory("Save")]
        public void SaveExistingTest() {
            var proj = VocaloidProject.CreateFromVpr(BASE_DIR + @"\Blank.vpr");
            proj.SaveToVpr(BASE_DIR + @"\Saved.vpr");
            if (!File.Exists(BASE_DIR + @"\Saved.vpr"))
                Assert.Fail(BASE_DIR + @"\Saved.vpr was not created");
            // TODO Compare contents
            File.Delete(BASE_DIR + @"\Saved.vpr");
        }

        [TestMethod, TestCategory("Save")]
        public void SaveNewTest() {
            var proj = new VocaloidProject();
            proj.SaveToVpr(BASE_DIR + @"\Created.vpr");
            File.Delete(BASE_DIR + @"\Created.vpr");
        }

        [TestMethod, TestCategory("Save")]
        public void SaveNewComplexTest() {
            // Create a project and save it
            var proj = new VocaloidProject();
            var singingTrack = new VocalTrack();
            var singingPart = new VocalPart();
            var dvqm = new DVQM();
            var firstNote = new VocalNote("ah", "@", 64, 64, 0, 100, dvqm, dvqm);
            singingPart.Glyphs.Add(0, firstNote);
            singingTrack.Events.Add(0, singingPart);
            proj.Tracks.Add(singingTrack);
            proj.SaveToVpr(BASE_DIR + @"\Created.vpr");

            // Load our project using CreateFromVpr
            var proj2 = VocaloidProject.CreateFromVpr(BASE_DIR + @"\Created.vpr");
            if ((proj2.Tracks.First().Events.First().Value as VocalPart).Glyphs.First().Value.MIDINote != 64)
                Assert.Fail("Comparison between saved and loaded projects failed");

            // Clean up and pass
            File.Delete(BASE_DIR + @"\Created.vpr");
        }
    }
}