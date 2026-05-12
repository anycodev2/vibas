using System.Text.Json.Nodes;
using shared.Documents;
using shared.Projects;

namespace shared.Serialization
{
    public class VibProjectSerializer : IVibSerializer<VibProject>
    {
        public VibProjectSerializer()
            => throw new NotImplementedException();

        public string Serialize(VibProject project)
            => throw new NotImplementedException();
        public VibProject Deserialize(string data)
            => throw new NotImplementedException();
        private JsonObject SerializeMetadata(VibProject project)
            => throw new NotImplementedException();
        private JsonArray SerializeDocuments(VibProject project)
            => throw new NotImplementedException();
        private string DeSerializeMetaData(JsonObject metadata)
            => throw new NotImplementedException();
        private List<VibDocument> DeSerializeDocuments(JsonArray documents)
            => throw new NotImplementedException();
    }
}