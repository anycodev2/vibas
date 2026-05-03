using shared.Blocks.Base;

namespace shared.Documents
{
    public class VibDocument
    {
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public bool isModified { get; set; }
        public string? Version { get; set; }
        public Guid Identifier { get; init; } = Guid.NewGuid();
        public List<VibBlock> Blocks { get; init; } = new List<VibBlock>();
        public List<VibConnection> Connections { get; init; } = new List<VibConnection>();
        //
        //This is almost the same as in VibProject, but for documents.
        //
        public VibDocument(string? fileName = null, string? filePath = null)
        {
            // This code checks if the file name is null or whitespace, and if so, it sets the FileName property to null.
            if (string.IsNullOrWhiteSpace(fileName))
            {
                FileName = null;
            }
            else
            {
                FileName = Path.HasExtension(fileName) ? fileName : fileName + ".vib";
            }
            FilePath = filePath;
            isModified = false;
        }
        public bool HasValidName()
        {
            // This class checks if the name of the project file is invalid 

            if (string.IsNullOrEmpty(FileName))
                return false;
            var name = FileName.Trim();
            //if the name is empty or longer than 128 characters, it's invalid
            if (name.Length == 0 || name.Length > 128)
                return false;
            //if the name contains any invalid characters, it's invalid
            if (name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                return false;
            var baseName = Path.GetFileNameWithoutExtension(name).Trim('.').ToUpperInvariant();
            // if name have only dots, it's invalid
            if (baseName.Trim('.').Length == 0)
                return false;
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
