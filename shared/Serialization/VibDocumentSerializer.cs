using shared.Blocks.Base;
using shared.Documents;
using System.Data;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace shared.Serialization
{
    public class VibDocumentSerializer : IVibSerializer<VibDocument>
    {
        public VibDocumentSerializer() { }

        public string Serialize(VibDocument document)
            => throw new NotImplementedException();
        public VibDocument Deserialize(string data)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters =
                {
                    new JsonStringEnumConverter(),
                    new BlockConverter()
                }
            };

            try
            {
                var blocksJson = JsonNode.Parse(data)!["blocks"]!.AsArray();
                var blocks = DeserializeBlocks(blocksJson);
                var blockMap = blocks.ToDictionary(block => block.Identifier);

                var connectionsJson = JsonNode.Parse(data)!["connections"]!.AsArray();
                var connections = DeserializeConnections(connectionsJson, blockMap);

                VibDocument? document = JsonSerializer.Deserialize<VibDocument>(data, options);

                if (document == null)
                    throw new JsonException("Deserialization returned null for VibDocument. Input might be null or empty.");

                document.Blocks.AddRange(blocks);
                document.Connections.AddRange(connections);

                return document;
            }
            catch (JsonException exception)
            {
                throw new JsonException($"Failed to deserialize VibDocument: {exception.Message}");
            }
        }
        private JsonObject SerializeBlocks(List<VibBlock> blocks)
            => throw new NotImplementedException();
        private JsonArray SerializeConnections(List<VibConnection> connections)
            => throw new NotImplementedException();
        private List<VibBlock> DeserializeBlocks(JsonArray data)
        {
            var blocks = new List<VibBlock>();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new BlockConverter() }
            };

            foreach (var node in data)
            {
                var block = JsonSerializer.Deserialize<VibBlock>(node!.ToJsonString(), options);
                blocks.Add(block!);
            }

            return blocks;
        }
        private List<VibConnection> DeserializeConnections(JsonArray data, Dictionary<Guid, VibBlock> blockMap)
        {
            var connections = new List<VibConnection>();

            foreach (var node in data)
            {
                var identifierId = Guid.Parse(node!["Identifier"]!.GetValue<string>());
                var sourceId = Guid.Parse(node!["Source"]!.GetValue<string>());
                var destinationId = Guid.Parse(node!["Destination"]!.GetValue<string>());
                var type = Enum.Parse<VibConnectionType>(node!["Type"]!.GetValue<string>());

                var sourceBlock = blockMap[sourceId];
                var destinationBlock = blockMap[destinationId];

                var connection = new VibConnection(sourceBlock, destinationBlock) 
                { 
                    Identifier = identifierId,
                    Type = type
                };
                connections.Add(connection);
            }

            return connections;
        }
    }
}