using shared.Blocks.Base;

namespace shared.Blocks.Types
{
    /// <summary>
    /// Represents a block that contains a sequence of statements to be executed.
    /// </summary>
    public class StatementBlock : VibCodeBlock {
        public StatementBlock() : base(BlockType.Statement) { }
    }
}
