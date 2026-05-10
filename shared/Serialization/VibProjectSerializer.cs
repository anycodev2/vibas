using shared.Documents;
using shared.Projects;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace shared.Serialization
{
    public class VibProjectSerializer : IVibSerializer<VibProject>
    {
        public VibProjectSerializer() { }

        public string Serialize(VibProject project) 
            => throw new NotImplementedException();
        public VibProject Deserialize(string data)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            try
            {
                VibProject? project = JsonSerializer.Deserialize<VibProject>(data, options);

                if (project == null)
                    throw new JsonException("Deserialization returned null for VibDocument. Input may be 'null' or empty.");

                return project;
            }
            catch (JsonException exception)
            {
                throw new JsonException($"Failed to deserialize VibDocument: {exception.Message}");
            }

        }
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
