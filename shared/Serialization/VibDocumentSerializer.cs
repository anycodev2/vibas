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
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            string baseJson = JsonSerializer.Serialize(document, options);
            var json = JsonNode.Parse(baseJson)!.AsObject();

            json["blocks"] = SerializeBlocks(document.Blocks);
            json["connections"] = SerializeConnections(document.Connections);

            return json.ToJsonString();
        }

        private JsonArray SerializeBlocks(List<VibBlock> blocks)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new BlockConverter() }
            };

            var jsonBlocks = JsonSerializer.Serialize(blocks, options);

            return JsonNode.Parse(jsonBlocks)!.AsArray();
        }
        private JsonArray SerializeConnections(List<VibConnection> connections)
        {
            var array = new JsonArray();

            foreach (var conn in connections)
            {
                array.Add(new JsonObject
                {
                    ["Identifier"] = conn.Identifier.ToString(),
                    ["Source"] = conn.Source.Identifier.ToString(),
                    ["Destination"] = conn.Destination.Identifier.ToString(),
                    ["Type"] = conn.Type.ToString()
                });
            }

            return array;
        }
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
                var nodeOptions = new JsonNodeOptions { PropertyNameCaseInsensitive = true };
                var root = JsonNode.Parse(data, nodeOptions)!;

                var jsonBlocks = root["blocks"]!.AsArray();
                var blocks = DeserializeBlocks(jsonBlocks);
                var blockMap = blocks.ToDictionary(block => block.Identifier);

                var jsonConnections = root["connections"]!.AsArray();
                var connections = DeserializeConnections(jsonConnections, blockMap);

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
                
                var sourceBlock = blockMap.GetValueOrDefault(sourceId);
                var destinationBlock = blockMap.GetValueOrDefault(destinationId);

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