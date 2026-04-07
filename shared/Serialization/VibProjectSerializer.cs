using System.Text.Json.Nodes;
using shared.Projects;

namespace shared.Serialization
{
    public class VibProjectSerializer
    {
        public VibProjectSerializer()
            => throw new NotImplementedException();

        public string Serializer(VibProject project) 
            => throw new NotImplementedException();
        public VibProject DeSerialize(string data) 
            => throw new NotImplementedException();
        private JsonObject SerializeMetadata(VibProject project) 
            => throw new NotImplementedException();
        private JsonArray SerializeDocuments(VibProject project) 
            => throw new NotImplementedException();
        private void DeSerializeMetaData(JsonObject metadata) 
            => throw new NotImplementedException();
        private void DeSerializeDocuments(JsonArray documents) 
            => throw new NotImplementedException();
    }
}
