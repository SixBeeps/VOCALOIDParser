using System.IO.Compression;
using System.Text.Json.Nodes;
using System.Runtime.InteropServices;
using System.Text.Json;

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

        public VocaloidProject() {
            Master = new MasterTrack();
            Tracks = new List<VocaloidTrack>();
        }

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
                    case 2:
                        project.Tracks.Add(new AIVocalTrack(track));
                        break;
                    default:
                        throw new NotImplementedException($"No track implementation for VOCALOID track type {trackType}");
                }
            }

            return project;
        }

        /// <summary>
        /// Saves the project to a .vpr file
        /// </summary>
        /// <param name="path">The path to save the project to</param>
        /// <param name="overwrite">Overwrites the file if it already exists</param>
        /// <exception cref="DirectoryNotFoundException">Thrown when a pre-existing project directory doesn't exist</exception>
        public void SaveToVpr(string path, bool overwrite = true) {
            // Create project directory if it does not already exist
            if (projectDirectory == null) {
                projectDirectory = Path.Join(workingDirectory, "Constructed_" + Guid.NewGuid().ToString());
                Directory.CreateDirectory(projectDirectory);
                Directory.CreateDirectory(Path.Combine(projectDirectory, "Project"));
            }

            // Open sequence.json for writing
            if (!Directory.Exists(projectDirectory)) throw new DirectoryNotFoundException("Project directory deleted before attempting to save");
            FileStream baseStream = new(Path.Combine(projectDirectory, "Project", "sequence.json"), FileMode.Create);
            Utf8JsonWriter jsonWriter = new(baseStream);
            jsonWriter.WriteStartObject();

            // Write voices
            jsonWriter.WriteStartArray("voices");
            foreach (var singer in SingerNames) {
                jsonWriter.WriteStartObject();
                jsonWriter.WriteString("compID", singer.Key);
                jsonWriter.WriteString("name", singer.Value);
                jsonWriter.WriteEndObject();
            }
            jsonWriter.WriteEndArray();
            jsonWriter.Flush();

            // Write master automation
            jsonWriter.WriteStartObject("masterTrack");
            Master.WriteJSON(jsonWriter);
            jsonWriter.WriteEndObject();

            // Write tracks
            jsonWriter.WriteStartArray("tracks");
            foreach (VocaloidTrack track in Tracks) {
                jsonWriter.WriteStartObject();
                if (track is VocalTrack) {
                    ((VocalTrack)track).WriteJSON(jsonWriter);
                } else if (track is AudioTrack) {
                    ((AudioTrack)track).WriteJSON(jsonWriter);
                }
                jsonWriter.WriteEndObject();
            }
            jsonWriter.WriteEndArray();
            jsonWriter.WriteEndObject();
            jsonWriter.Flush();

            // Clean up and package into .vpr file
            jsonWriter.Dispose();
            baseStream.Close();
            if (overwrite && File.Exists(path)) File.Delete(path);
            ZipFile.CreateFromDirectory(projectDirectory, path);
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