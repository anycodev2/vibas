using shared.Blocks.Base;

namespace shared.Blocks.Types
{
    /// <summary>
    /// Represents a block that handles input and output operations.
    /// </summary>
    public class IOBlock : VibBlock {
        public IOBlock() : base(BlockType.IO) { }
    }
}
