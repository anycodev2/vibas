using shared.Blocks.Base;

namespace shared.Blocks.Types
{
    /// <summary>
    /// Represents a block that indicates the start of an algorithm.
    /// </summary>
    public class StartBlock : VibBlock {
        public StartBlock() : base(BlockType.Start) { }
    }
}
