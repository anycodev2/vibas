using shared.Blocks.Base;
using System.Text.Json.Serialization;

namespace shared.Documents
{
    public class VibDocument
    {
        [JsonPropertyName("name")]
        public string? FileName { get; set; }
        [JsonPropertyName("path")]
        public string? FilePath { get; set; }
        [JsonIgnore]
        public bool isModified { get; set; }
        public string? Version { get; set; }
        public Guid Identifier { get; init; } = Guid.NewGuid();
        public List<VibBlock> Blocks { get; set; } = new List<VibBlock>();
        public List<VibConnection> Connections { get; set; } = new List<VibConnection>();

        public VibDocument(string? fileName = null, string? filePath = null, string? version = null) 
        {
            FileName = fileName;
            FilePath = filePath;
            Version = version;
        }

        public bool HasValidName()
            => throw new NotImplementedException();
    }
}