namespace shared.Blocks.Base
{
    /// <summary>
    /// Represents an abstract block that contains code content within a vib document.
    /// </summary>
    /// <remarks>This class serves as a base for blocks that encapsulate code snippets or fragments. Inherit
    /// from this class to implement specific types of code blocks as needed.</remarks>
    public abstract class VibCodeBlock : VibBlock
    {
        public string? Code { get; set; }

        protected VibCodeBlock(BlockType type) : base(type) { }
    }
}
