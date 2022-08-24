using System.IO.Compression;
using System.Text.Json.Nodes;
using System.Runtime.InteropServices;

namespace SixBeeps.VOCALOIDParser
{
    public class VocaloidProject
    {
        private static string workingDirectory = Path.Combine(Path.GetTempPath(), "VOCALOIDParser");
        private string? projectDirectory;

        /// <summary>
        /// Collection of singer IDs to names.
        /// </summary>
        public static Dictionary<string, string> SingerNames { get; private set; } = new Dictionary<string, string>();

        /// <summary>
        /// Information about master effects such as tempo.
        /// </summary>
        public MasterTrack Master { get; private set; }

        /// <summary>
        /// List of all tracks in the project.
        /// </summary>
        public List<VocaloidTrack> Tracks;

        public List<VocalTrack> VocalTracks => (from track in Tracks where track is VocalTrack select (VocalTrack)track).ToList();
        public List<AudioTrack> AudioTracks => (from track in Tracks where track is AudioTrack select (AudioTrack)track).ToList();

        /// <summary>
        /// Initial tempo of the project.
        /// </summary>
        public float BPM => (Master.TempoTrack.UseGlobal ? Master.TempoTrack.GlobalValue : Master.TempoTrack.Evaluate(0)) / 100f;

        /// <summary>
        /// Creates an instance of <c>VocaloidProject</c> given a path to the .vpr file.
        /// </summary>
        /// <param name="path">Path to the Vocaloid project</param>
        /// <returns>The newly-created project instance</returns>
        /// <exception cref="NotImplementedException">Thrown when a nonbinary track type is found</exception>

        public static VocaloidProject CreateFromVpr(string path)
        {
            var project = new VocaloidProject
            {
                // Extract to directory if needed
                projectDirectory = Path.Join(workingDirectory, Path.GetFileNameWithoutExtension(path))
            };
            project.ClearProjectDirectory();
            ZipFile.ExtractToDirectory(path, project.projectDirectory);

            // Create JsonNode from sequence.json
            string jsonFile;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) // Thanks Windows!
                jsonFile = Path.Combine(project.projectDirectory, "Project", "sequence.json");
            else
                jsonFile = project.projectDirectory + "/Project\\sequence.json";
            JsonNode docNode = JsonNode.Parse(File.ReadAllText(jsonFile));

            // Obtain list of singers and their compIDs
            foreach (JsonNode info in docNode["voices"].AsArray())
            {
                SingerNames.TryAdd(info["compID"].ToString(), info["name"].ToString());
            }

            // Initialize global automation
            project.Master = new MasterTrack(docNode["masterTrack"]);

            // Create tracks
            project.Tracks = new List<VocaloidTrack>();
            foreach (JsonNode track in docNode["tracks"].AsArray())
            {
                int trackType = track["type"].GetValue<int>();
                switch(trackType)
                {
                    case 0:
                        project.Tracks.Add(new VocalTrack(track));
                        break;
                    case 1:
                        project.Tracks.Add(new AudioTrack(track));
                        break;
                    default:
                        throw new NotImplementedException($"No track implementation for VOCALOID track type {trackType}");
                }
            }

            return project;
        }

        /// <summary>
        /// Change the directory which Vocaloid projects are extracted to. The default directory is the OS's temp path. Use this if you are in an environment in which the temp path can't be accessed.
        /// </summary>
        /// <param name="directory">The directory to work in</param>
        /// <exception cref="DirectoryNotFoundException">Thrown whenever the directory doesn't exist. This function does not create the directory.</exception>
        public static void SetWorkingDirectory(string directory)
        {
            if (!Directory.Exists(directory)) throw new DirectoryNotFoundException("Attempted to set working directory to invalid path " + directory);
            workingDirectory = directory;
        }

        internal void ClearProjectDirectory()
        {
            if (Directory.Exists(projectDirectory)) Directory.Delete(projectDirectory, true);
        }
    }
}