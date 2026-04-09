using System.Text.Json.Serialization;

namespace shared.Blocks
{
    /// <summary>
    /// Specifies the types of blocks used inside a vib document, such as start, stop, conditional, 
    /// statement, and input/output blocks.
    /// </summary>
    /// <remarks>Use this enumeration to identify or differentiate between various block types when
    /// constructing or analyzing flowcharts. Each value represents a distinct logical or operational element</remarks>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum BlockType
    {
        Start,
        Stop,
        Conditional,
        Statement,
        IO
    }
}
