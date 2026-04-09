using shared.Blocks.Types;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace shared.Blocks.Base
{
    /// <summary>
    /// Represents the base class for a block in a vib document, providing common
    /// properties for identification, type, and extension data.
    /// </summary>
    /// <remarks>This class is intended to be inherited by specific block implementations within the vibas
    /// system. It provides a unique identifier, a block type, and supports extension data for additional properties not
    /// defined in the base class. The extension data enables forward compatibility and flexible serialization
    /// scenarios.</remarks>
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
    [JsonDerivedType(typeof(StartBlock), typeDiscriminator: "StartBlock")]
    [JsonDerivedType(typeof(StopBlock), typeDiscriminator: "StopBlock")]
    [JsonDerivedType(typeof(StatementBlock), typeDiscriminator: "StatementBlock")]
    [JsonDerivedType(typeof(ConditionalBlock), typeDiscriminator: "ConditionalBlock")]
    [JsonDerivedType(typeof(IOBlock), typeDiscriminator: "InteractionBlock")]
    public abstract class VibBlock
    {
        public BlockType Type { get; init; }
        public Guid Identifier { get; init; } = Guid.NewGuid();

        [JsonExtensionData]
        public Dictionary<string, JsonElement>? ExtensionData { get; set; }

        protected VibBlock(BlockType type)
        {
            Type = type;
        }
    }
}
