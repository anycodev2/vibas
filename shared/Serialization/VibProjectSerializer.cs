using shared.Blocks.Base;
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
        {
            string json = JsonSerializer.Serialize(project);

            return json;
        }
        private JsonObject SerializeMetadata(VibProject project)
        => throw new NotImplementedException();
        private JsonArray SerializeDocuments(VibProject project)
            => throw new NotImplementedException();
        public VibProject Deserialize(string data)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            try
            {
                VibProject? project = JsonSerializer.Deserialize<VibProject>(data, options);

                if (project == null)
                    throw new JsonException("Deserialization returned null for VibProject. Input might be null or empty.");

                var dataAsObject = JsonNode.Parse(data) as JsonObject;
                project.FileName = DeserializeMetaData(dataAsObject);


                var docsArray = dataAsObject["documents"]?.AsArray();
                if (docsArray != null)
                {
                    project.Documents.AddRange(DeserializeDocuments(docsArray));
                }

                return project;
            }
            catch (JsonException exception)
            {
                throw new JsonException($"Failed to deserialize VibDocument: {exception.Message}");
            }
        }
        private string DeserializeMetaData(JsonObject metadata)
        {
            var name = metadata["name"].GetValue<string>();

            return Path.HasExtension(name) ? name : name + ".vibproj";
        }
        private List<VibDocument> DeserializeDocuments(JsonArray documents)
        {
            var list = new List<VibDocument>();

            foreach (var node in documents)
            {
                string path = node!.GetValue<string>();

                var doc = new VibDocument(Path.GetFileName(path), path);
                list.Add(doc);
            }

            return list;
        }
    }
}