using shared.Blocks.Base;
using shared.Documents;
using System.Text.Json.Nodes;

namespace shared.Serialization
{
    public class VibDocumentSerializer : IVibSerializer<VibDocument>
    {
        public VibDocumentSerializer()
            => throw new NotImplementedException();

        public string Serialize(VibDocument document)
            => throw new NotImplementedException();
        public VibDocument Deserialize(string data)
            => throw new NotImplementedException();
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