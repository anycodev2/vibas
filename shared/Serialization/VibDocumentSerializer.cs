using shared.Blocks.Base;
using shared.Documents;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

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
                PropertyNameCaseInsensitive = true,
            };

            VibDocument? doc = JsonSerializer.Deserialize<VibDocument>(data, options);

            if (doc == null)
            {
                // Niech narazie będzie tak
                throw new JsonException("Deserializacja zakończyła się niepowodzeniem");
            }
            else
            {
                return doc;
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
