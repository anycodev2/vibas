using shared.Blocks.Base;
using shared.Documents;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace shared.Serialization
{
    public class VibDocumentSerializer : IVibSerializer<VibDocument>
    {
        public VibDocumentSerializer() {}

        public string Serialize(VibDocument document)
            => throw new NotImplementedException();
        public VibDocument Deserialize(string data)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            VibDocument? doc = JsonSerializer.Deserialize<VibDocument>(data, options);

            if (doc == null)
                throw new JsonException("Deserializacja zakończyła się niepowodzeniem");

            return doc;
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
