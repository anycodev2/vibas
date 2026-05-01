using shared.Blocks.Base;

namespace shared.Documents
{
    public class VibConnection
    {
        public Guid Source { get; set; }
        public Guid Destination { get; set; }
        public Guid Identifier { get; init; } = Guid.NewGuid();
        public VibConnectionType Type { get; set; } = VibConnectionType.Unconditional;

        public VibConnection() { }
        public VibConnection(VibBlock source, VibBlock destination)
        {
            Source = source.Identifier;
            Destination = destination.Identifier;
        }
    }
}
