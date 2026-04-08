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

        public VibDocument(string? fileName = null, string? filePath = null) 
            => throw new NotImplementedException();

        public bool HasValidName()
            => throw new NotImplementedException();
    }
}
