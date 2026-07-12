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
    [JsonConverter(typeof(BlockConverter))]
    public abstract class Block
    {
        public abstract string Type { get; }
        public Guid Identifier { get; set; }
    }
    public class BlockConverter : JsonConverter<VibBlock>
    {
        public override VibBlock? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                using var doc = JsonDocument.ParseValue(ref reader);
                var root = doc.RootElement;

                if (!root.TryGetProperty("Type", out var typeProp) && !root.TryGetProperty("type", out typeProp))
                {
                    throw new KeyNotFoundException();
                }

                var type = typeProp.GetString();

                Type concreteType = type switch
                {
                    nameof(BlockType.Start) => typeof(StartBlock),
                    nameof(BlockType.Stop) => typeof(StopBlock),
                    nameof(BlockType.Statement) => typeof(StatementBlock),
                    nameof(BlockType.Conditional) => typeof(ConditionalBlock),
                    nameof(BlockType.IO) => typeof(IOBlock),
                    _ => throw new JsonException($"Unknown block type: {type}")
                };

                return (VibBlock?)JsonSerializer.Deserialize(root.GetRawText(), concreteType, options);
            }
            catch (KeyNotFoundException)
            {
                throw new JsonException("Block is missing required 'Type' discriminator.");
            }
        }

        public override void Write(Utf8JsonWriter writer, VibBlock value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, (object)value, options);
        }
    }
}