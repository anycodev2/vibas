using FluentAssertions;
using shared.Blocks.Types;
using shared.Documents;
using shared.Serialization;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Shared.Tests.Serialization
{
    public class VibDocumentSerializerTests
    {
        private readonly VibDocumentSerializer _serializer = new();

        [Fact]
        public void Serialize_ShouldReturnValidJson_WithNameVersionBlocksConnections()
        {
            var doc = new VibDocument("algo.vib", "path/algo.vib", "v0.05");

            var json = _serializer.Serialize(doc);
            var node = JsonNode.Parse(json);

            node.Should().NotBeNull();
            node!["name"].Should().NotBeNull();
            node["version"].Should().NotBeNull();
            node["blocks"].Should().NotBeNull();
            node["connections"].Should().NotBeNull();
        }

        [Fact]
        public void Serialize_ShouldThrowArgumentNullException_WhenDocumentIsNull()
        {
            Action act = () => _serializer.Serialize(null!);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Serialize_ShouldProduceEmptyArrays_WhenDocumentHasNoBlocksOrConnections()
        {
            var doc = new VibDocument("empty.vib", "empty.vib");

            var json = _serializer.Serialize(doc);
            var node = JsonNode.Parse(json);

            node!["blocks"]!.AsArray().Should().BeEmpty();
            node["connections"]!.AsArray().Should().BeEmpty();
        }

        [Fact]
        public void Serialize_ShouldIncludeTypeDiscriminator_ForEachBlock()
        {
            var doc = new VibDocument("doc.vib", "doc.vib");
            doc.Blocks.Add(new StartBlock());
            doc.Blocks.Add(new StopBlock());

            var json = _serializer.Serialize(doc);
            var node = JsonNode.Parse(json);

            var blocks = node!["blocks"]!.AsArray();
            blocks.Should().AllSatisfy(b =>
                b!["$type"].Should().NotBeNull("every block needs a $type discriminator"));
        }

        [Fact]
        public void Serialize_ShouldWriteCorrectType_ForStartBlock()
        {
            var doc = new VibDocument("doc.vib", "doc.vib");
            doc.Blocks.Add(new StartBlock());

            var json = _serializer.Serialize(doc);
            var node = JsonNode.Parse(json);
            var block = node!["blocks"]!.AsArray()[0]!;

            block["$type"]!.GetValue<string>().Should().Be("StartBlock");
        }

        [Fact]
        public void Serialize_ShouldWriteCorrectType_ForStopBlock()
        {
            var doc = new VibDocument("doc.vib", "doc.vib");
            doc.Blocks.Add(new StopBlock());

            var block = JsonNode.Parse(_serializer.Serialize(doc))!
                                ["blocks"]!.AsArray()[0]!;

            block["$type"]!.GetValue<string>().Should().Be("StopBlock");
        }

        [Fact]
        public void Serialize_ShouldWriteCorrectType_ForConditionalBlock()
        {
            var doc = new VibDocument("doc.vib", "doc.vib");
            doc.Blocks.Add(new ConditionalBlock());

            var block = JsonNode.Parse(_serializer.Serialize(doc))!
                                ["blocks"]!.AsArray()[0]!;

            block["$type"]!.GetValue<string>().Should().Be("ConditionalBlock");
        }

        [Fact]
        public void Serialize_ShouldWriteIdentifier_ForEachBlock()
        {
            var start = new StartBlock();
            var doc = new VibDocument("doc.vib", "doc.vib");
            doc.Blocks.Add(start);

            var block = JsonNode.Parse(_serializer.Serialize(doc))!
                                ["blocks"]!.AsArray()[0]!;

            block["identifier"]!.GetValue<string>()
                 .Should().Be(start.Identifier.ToString());
        }

        [Fact]
        public void Serialize_ShouldWriteCode_ForCodeBlock()
        {
            var statement = new StatementBlock { Code = "x = x + 1" };
            var doc = new VibDocument("doc.vib", "doc.vib");
            doc.Blocks.Add(statement);

            var block = JsonNode.Parse(_serializer.Serialize(doc))!
                                ["blocks"]!.AsArray()[0]!;

            block["code"]!.GetValue<string>().Should().Be("x = x + 1");
        }

        [Fact]
        public void Serialize_ShouldPreserveExtensionData_WhenPresent()
        {
            var block = new StatementBlock
            {
                ExtensionData = new Dictionary<string, JsonElement>
                {
                    ["x"] = JsonSerializer.SerializeToElement(42.5),
                    ["color"] = JsonSerializer.SerializeToElement("#FF0000")
                }
            };
            var doc = new VibDocument("doc.vib", "doc.vib");
            doc.Blocks.Add(block);

            var json = _serializer.Serialize(doc);
            var blockNode = JsonNode.Parse(json)!["blocks"]!.AsArray()[0]!;

            blockNode["x"]!.GetValue<double>().Should().BeApproximately(42.5, 0.001);
            blockNode["color"]!.GetValue<string>().Should().Be("#FF0000");
        }

        [Fact]
        public void Serialize_ShouldWriteConnection_WithSourceDestinationAndType()
        {
            var source = new StartBlock();
            var destination = new StopBlock();
            var doc = new VibDocument("doc.vib", "doc.vib");
            doc.Blocks.Add(source);
            doc.Blocks.Add(destination);
            doc.Connections.Add(new VibConnection
            {
                Identifier = Guid.NewGuid(),
                Source = source.Identifier,
                Destination = destination.Identifier,
                Type = VibConnectionType.Unconditional
            });

            var node = JsonNode.Parse(_serializer.Serialize(doc))!;
            var conn = node["connections"]!.AsArray()[0]!;

            conn["source"]!.GetValue<string>().Should().Be(source.Identifier.ToString());
            conn["destination"]!.GetValue<string>().Should().Be(destination.Identifier.ToString());
            conn["type"]!.GetValue<string>().Should().Be("Unconditional");
        }

        [Fact]
        public void Serialize_ShouldWriteAllConnectionTypes()
        {
            foreach (var type in Enum.GetValues<VibConnectionType>())
            {
                var doc = new VibDocument("doc.vib", "doc.vib", "v0.05");
                var source = new StartBlock(); var destination = new StopBlock();
                doc.Blocks.Add(source); doc.Blocks.Add(destination);
                doc.Connections.Add(new VibConnection
                {
                    Identifier = Guid.NewGuid(),
                    Source = source.Identifier,
                    Destination = destination.Identifier,
                    Type = type
                });

                var conn = JsonNode.Parse(_serializer.Serialize(doc))!
                                   ["connections"]!.AsArray()[0]!;

                conn["type"]!.GetValue<string>().Should().Be(type.ToString());
            }
        }

        [Fact]
        public void Deserialize_ShouldReconstructStartBlock_FromJson()
        {
            var json = """
            {
                "name": "doc.vib",
                "version": "1.0",
                "blocks": [
                    {
                        "$type": "StartBlock",
                        "Type": "Start",
                        "Identifier": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"
                    }
                ],
                "connections": []
            }
            """;

            var doc = _serializer.Deserialize(json);

            doc.Blocks.Should().HaveCount(1);
            doc.Blocks[0].Should().BeOfType<StartBlock>();
            doc.Blocks[0].Identifier.Should().Be(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));
        }

        [Fact]
        public void Deserialize_ShouldReconstructAllBlockTypes()
        {
            var json = """
            {
                "name": "doc.vib", "version": "1.0",
                "blocks": [
                    { "$type": "StartBlock",       "Type": "Start",       "Identifier": "aaaaaaaa-0000-0000-0000-000000000000" },
                    { "$type": "StopBlock",        "Type": "Stop",        "Identifier": "bbbbbbbb-0000-0000-0000-000000000000" },
                    { "$type": "StatementBlock",   "Type": "Statement",   "Identifier": "cccccccc-0000-0000-0000-000000000000", "Code": "x=1" },
                    { "$type": "ConditionalBlock", "Type": "Conditional", "Identifier": "dddddddd-0000-0000-0000-000000000000", "Code": "x>0" },
                    { "$type": "InteractionBlock", "Type": "IO", "Identifier": "eeeeeeee-0000-0000-0000-000000000000", "Code": "print(x)" }
                ],
                "connections": []
            }
            """;

            var doc = _serializer.Deserialize(json);

            doc.Blocks.Should().HaveCount(5);
            doc.Blocks[0].Should().BeOfType<StartBlock>();
            doc.Blocks[1].Should().BeOfType<StopBlock>();
            doc.Blocks[2].Should().BeOfType<StatementBlock>();
            doc.Blocks[3].Should().BeOfType<ConditionalBlock>();
            doc.Blocks[4].Should().BeOfType<IOBlock>();
        }

        [Fact]
        public void Deserialize_ShouldRestoreCode_ForCodeBlocks()
        {
            var json = """
            {
                "name": "doc.vib", "version": "1.0",
                "blocks": [
                    {
                        "$type": "StatementBlock",
                        "Type": "Statement",
                        "Identifier": "cccccccc-0000-0000-0000-000000000000",
                        "Code": "result = a + b"
                    }
                ],
                "connections": []
            }
            """;

            var doc = _serializer.Deserialize(json);

            doc.Blocks[0].Should().BeOfType<StatementBlock>()
               .Which.Code.Should().Be("result = a + b");
        }

        [Fact]
        public void Deserialize_ShouldPreserveExtensionData_WhenPresent()
        {
            var json = """
            {
                "name": "doc.vib", "version": "1.0",
                "blocks": [
                    {
                        "$type": "StartBlock",
                        "Type": "Start",
                        "Identifier": "aaaaaaaa-0000-0000-0000-000000000000",
                        "x": 120.0,
                        "color": "#A78BFA"
                    }
                ],
                "connections": []
            }
            """;

            var doc = _serializer.Deserialize(json);
            var block = doc.Blocks[0];

            block.ExtensionData.Should().NotBeNull();
            block.ExtensionData!["x"].GetDouble().Should().BeApproximately(120.0, 0.001);
            block.ExtensionData["color"].GetString().Should().Be("#A78BFA");
        }

        [Fact]
        public void Deserialize_ShouldThrow_WhenBlockTypeDiscriminatorIsUnknown()
        {
            var json = """
            {
                "name": "doc.vib", "version": "1.0",
                "blocks": [
                    { "$type": "NonExistentBlock", "Identifier": "aaaaaaaa-0000-0000-0000-000000000000" }
                ],
                "connections": []
            }
            """;

            Action act = () => _serializer.Deserialize(json);
            act.Should().Throw<JsonException>();
        }

        [Fact]
        public void Deserialize_ShouldReconstructConnection_WithCorrectFields()
        {
            var json = """
            {
                "name": "doc.vib", "version": "1.0",
                "blocks": [],
                "connections": [
                    {
                        "Identifier": "11111111-0000-0000-0000-000000000000",
                        "Source":      "aaaaaaaa-0000-0000-0000-000000000000",
                        "Destination": "bbbbbbbb-0000-0000-0000-000000000000",
                        "Type":        "Unconditional"
                    }
                ]
            }
            """;

            var doc = _serializer.Deserialize(json);
            var conn = doc.Connections[0];

            conn.Identifier.Should().Be(Guid.Parse("11111111-0000-0000-0000-000000000000"));
            conn.Source.Should().Be(Guid.Parse("aaaaaaaa-0000-0000-0000-000000000000"));
            conn.Destination.Should().Be(Guid.Parse("bbbbbbbb-0000-0000-0000-000000000000"));
            conn.Type.Should().Be(VibConnectionType.Unconditional);
        }

        [Theory]
        [InlineData("Truthy", VibConnectionType.Truthy)]
        [InlineData("Falsy", VibConnectionType.Falsy)]
        [InlineData("Unconditional", VibConnectionType.Unconditional)]
        public void Deserialize_ShouldRestoreAllConnectionTypes(
            string typeString, VibConnectionType expected)
        {
            var json = $$"""
            {
                "name": "doc.vib", "version": "1.0",
                "blocks": [],
                "connections": [
                    {
                        "Identifier": "11111111-0000-0000-0000-000000000000",
                        "Source":      "aaaaaaaa-0000-0000-0000-000000000000",
                        "Destination": "bbbbbbbb-0000-0000-0000-000000000000",
                        "Type": "{{typeString}}"
                    }
                ]
            }
            """;

            var doc = _serializer.Deserialize(json);
            doc.Connections[0].Type.Should().Be(expected);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("not-json")]
        [InlineData("{broken json")]
        public void Deserialize_ShouldThrowJsonException_WhenInputIsMalformed(string bad)
        {
            Action act = () => _serializer.Deserialize(bad);
            act.Should().ThrowExactly<JsonException>();
        }

        [Fact]
        public void Deserialize_ShouldThrowArgumentNullException_WhenInputIsNull()
        {
            Action act = () => _serializer.Deserialize(null!);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void RoundTrip_ShouldPreserveBlocksAndConnections()
        {
            var doc = new VibDocument("rt.vib", "path/rt.vib");
            var start = new StartBlock();
            var stop = new StopBlock();
            var stmt = new StatementBlock { Code = "n = 10" };

            doc.Blocks.Add(start);
            doc.Blocks.Add(stop);
            doc.Blocks.Add(stmt);
            doc.Connections.Add(new VibConnection
            {
                Identifier = Guid.NewGuid(),
                Source = start.Identifier,
                Destination = stmt.Identifier,
                Type = VibConnectionType.Unconditional
            });
            doc.Connections.Add(new VibConnection
            {
                Identifier = Guid.NewGuid(),
                Source = stmt.Identifier,
                Destination = stop.Identifier,
                Type = VibConnectionType.Unconditional
            });

            var json = _serializer.Serialize(doc);
            var restored = _serializer.Deserialize(json);

            restored.Blocks.Should().HaveCount(3);
            restored.Blocks[0].Should().BeOfType<StartBlock>();
            restored.Blocks[1].Should().BeOfType<StopBlock>();
            restored.Blocks[2].Should().BeOfType<StatementBlock>()
                    .Which.Code.Should().Be("n = 10");

            restored.Connections.Should().HaveCount(2);
            restored.Connections[0].Source.Should().Be(start.Identifier);
            restored.Connections[1].Destination.Should().Be(stop.Identifier);
        }

        [Fact]
        public void RoundTrip_ShouldPreserveExtensionData()
        {
            var block = new StartBlock
            {
                ExtensionData = new Dictionary<string, JsonElement>
                {
                    ["x"] = JsonSerializer.SerializeToElement(55.0),
                    ["color"] = JsonSerializer.SerializeToElement("#00FF00")
                }
            };
            var doc = new VibDocument("rt.vib", "rt.vib");
            doc.Blocks.Add(block);

            var restored = _serializer.Deserialize(_serializer.Serialize(doc));

            restored.Blocks[0].ExtensionData.Should().NotBeNull();
            restored.Blocks[0].ExtensionData!["x"].GetDouble()
                    .Should().BeApproximately(55.0, 0.001);
            restored.Blocks[0].ExtensionData["color"].GetString()
                    .Should().Be("#00FF00");
        }
    }
}
