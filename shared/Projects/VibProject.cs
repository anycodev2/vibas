using shared.Documents;
using System.IO;
namespace shared.Projects
{
    public class VibProject
    {
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public string? Version { get; set; }
        public List<VibDocument> Documents { get; }

        /// <summary>
        /// Initializes a new instance of the VibProject class with the specified file name and file path.
        /// </summary>
        /// <param name="fileName">The name of the project file to associate with this instance. Can be null if not specified.</param>
        /// <param name="filePath">The full path to the project file. Can be null if not specified.</param>
        public VibProject(string? fileName = null, string? filePath = null)
        {
            // If the FileName is Test, the constructor will add the extension if it's missing. 
            // then the filename will be Test.vibproj
            if (string.IsNullOrWhiteSpace(fileName))
            {
                FileName = null;
            }
            else
            {
                FileName = Path.HasExtension(fileName) ? fileName : fileName + ".vibproj";
            }
            FilePath = filePath;
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
        {
            // This class checks if the name of the project file is invalid 
            if (string.IsNullOrWhiteSpace(FileName))
                return false;
            var name = FileName.Trim();
            //if the name is empty or longer than 128 characters, it's invalid
            if (name.Length == 0 || name.Length > 128) return false;
            //if the name contains any invalid characters, it's invalid
            if (name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0) return false;
            var baseName = Path.GetFileNameWithoutExtension(name).ToUpperInvariant();
            // if name have only dots, it's invalid
            if (baseName.Trim('.').Length == 0) return false;

            // if the name is a reserved name, it's invalid
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
