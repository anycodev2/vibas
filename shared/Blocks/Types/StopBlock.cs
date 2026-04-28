using shared.Blocks.Base;

namespace shared.Blocks.Types
{
    /// <summary>
    /// Represents a block that indicates the end of an algorithm.
    /// </summary>
    public class StopBlock : VibBlock {
        public StopBlock() : base(BlockType.Stop) { }
    }
}
