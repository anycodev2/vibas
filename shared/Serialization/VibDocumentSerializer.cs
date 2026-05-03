using shared.Blocks.Base;
using shared.Documents;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace shared.Serialization
{
    public class VibDocumentSerializer : IVibSerializer<VibDocument>
    {
        public VibDocumentSerializer() {}

        public string Serialize(VibDocument document)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            string json = JsonSerializer.Serialize(document, options);

            ArgumentNullException.ThrowIfNull(document);

            return json;
        }
        public VibDocument Deserialize(string data)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            };

            try 
            {
                VibDocument? doc = JsonSerializer.Deserialize<VibDocument>(data, options);

                if (doc == null)
                    throw new JsonException("Deserialization returned null for VibDocument. Input may be 'null' or empty.");

                return doc;
            }
            catch(JsonException exception) 
            {
                throw new JsonException($"Failed to deserialize VibDocument: {exception.Message}");
            }
        }
        private JsonObject SerializeBlocks(List<VibBlock> blocks)
            => throw new NotImplementedException();
        private JsonArray SerializeConnections(List<VibConnection> connections)
            => throw new NotImplementedException();
        private List<VibBlock> DeserializeBlocks(JsonObject data)
            => throw new NotImplementedException();
        private List<VibConnection> DeserializeConnections(JsonArray data)
            => throw new NotImplementedException();
    }
}
