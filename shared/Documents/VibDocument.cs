using shared.Blocks.Base;
using System.Text.Json.Serialization;

namespace shared.Documents
{
    public class VibDocument
    {
        public string? FileName { get; set; }
        [JsonPropertyName("name")]
        public string? FilePath { get; set; }
        [JsonPropertyName("path")]
        public bool isModified { get; set; }
        public string? Version { get; set; }
        public Guid Identifier { get; init; } = Guid.NewGuid();
        public List<VibBlock> Blocks { get; init; } = new List<VibBlock>();
        public List<VibConnection> Connections { get; set; } = new List<VibConnection>();

        /// <summary>
        /// Initializes a new instance of the VibDocument class with the specified file name and file path.
        /// </summary>
        /// <param name="fileName">The name of the document file to associate with this instance. Can be null if not specified.</param>
        /// <param name="filePath">The full path to the document file. Can be null if not specified.</param>

        public VibDocument(String? fileName = null, String? filePath = null, String? version = null)
        {
            if (String.IsNullOrWhiteSpace(fileName))
            {
                FileName = null;
            }
            else
            {
                FileName = Path.HasExtension(fileName) ? fileName : fileName + ".vib";
            }
            FilePath = filePath;
            Version = version;
            isModified = false;
        }

        public bool HasValidName()
        {
            if (String.IsNullOrEmpty(FileName))
                return false;
            var name = FileName.Trim();
            //If the name is empty or longer than 128 characters, it's invalid
            if (name.Length == 0 || name.Length > 128)
                return false;
            //If the name contains any invalid characters, it's invalid
            if (name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                return false;
            var baseName = Path.GetFileNameWithoutExtension(name).Trim('.').ToUpperInvariant();
            //If name have only dots, it's invalid
            if (baseName.Trim('.').Length == 0)
                return false;
            //If the name is a reserved name, it's invalid
            var reserved = new[]
            {
                "CON", "PRN", "AUX", "NUL",
                "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9",
                "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9"
            };
            return Array.IndexOf(reserved, baseName) < 0;
        } 
    }
}
