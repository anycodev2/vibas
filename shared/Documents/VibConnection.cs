using shared.Blocks.Base;

namespace shared.Documents
{
    public class VibConnection
    {
        public VibBlock Source { get; set; }
        public VibBlock Destination { get; set; }
        public Guid Identifier { get; init; } = Guid.NewGuid();
        public VibConnectionType Type { get; set; } = VibConnectionType.Unconditional;

        public VibConnection() { }
        public VibConnection(VibBlock source, VibBlock destination)
        {
            Source = source;
            Destination = destination;
        }
    }
}