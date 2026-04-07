using shared.Blocks.Base;

namespace shared.Blocks.Types
{
    /// <summary>
    /// Represents a block that contains a conditional statement,
    /// which evaluates a condition and executes different blocks based on the result.
    /// </summary>
    public class ConditionalBlock : VibCodeBlock {
        public ConditionalBlock() : base(BlockType.Conditional) { }
    }
}
