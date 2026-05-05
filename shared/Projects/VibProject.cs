using shared.Documents;
using System.Text.Json.Serialization;

namespace shared.Projects
{
    public class VibProject
    {
        [JsonPropertyName("name")]
        public string? FileName { get; set; }
        [JsonPropertyName("path")]
        public string? FilePath { get; set; }
        public string? Version { get; set; }
        public List<VibDocument> Documents { get; set; }

        /// <summary>
        /// Initializes a new instance of the VibProject class with the specified file name and file path.
        /// </summary>
        /// <param name="fileName">The name of the project file to associate with this instance. Can be null if not specified.</param>
        /// <param name="filePath">The full path to the project file. Can be null if not specified.</param>
        public VibProject(string? fileName = null, string? filePath = null, string? version = null)
        {
            FileName = fileName;
            FilePath = filePath;
            Version = version;
            Documents = new List<VibDocument>();
        }

        /// <summary>
        /// Determines whether the current instance has a valid name.
        /// Validation rules:
        ///     Not null
        ///     Less than 128 characters
        ///     Should not contain invalid characters (e.g., \ / : * ? " < > |)
        ///     Should not consist solely of whitespace characters
        ///     Should not be a reserved name (e.g., "CON", "PRN", "AUX", "NUL", "COM1" to "COM9", "LPT1" to "LPT9")
        /// </summary>
        /// <returns>true if the name is valid; otherwise, false.</returns>
        public bool HasValidName()
            => throw new NotImplementedException();
    }
}
