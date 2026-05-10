using FluentAssertions;
using shared.Documents;
using shared.Projects;
using shared.Serialization;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Shared.Tests.Serialization
{
    public class VibProjectSerializerTests
    {
        private readonly VibProjectSerializer _serializer = new();

        [Fact]
        public void Serialize_ShouldReturnValidJson_WithMetadataAndDocumentPaths()
        {
            var project = new VibProject("MyProject", "C:/projects/test.vibproj")
            {
                Version = "1.2.3"
            };
            project.Documents.Add(new VibDocument("algo1.vib", "C:/projects/algorithms/algo1.vib"));
            project.Documents.Add(new VibDocument("algo2.vib", "C:/projects/algorithms/algo2.vib"));

            var json = _serializer.Serialize(project);
            var node = JsonNode.Parse(json);

            node.Should().NotBeNull();
            node!["name"]!.GetValue<string>().Should().Be("MyProject");
            node["version"]!.GetValue<string>().Should().Be("1.2.3");

            var docs = node["documents"]!.AsArray();
            docs.Should().HaveCount(2);
            docs.Select(x => x!.GetValue<string>()).Should().BeEquivalentTo(new[]
            {
                "C:/projects/algorithms/algo1.vib",
                "C:/projects/algorithms/algo2.vib"
            });
        }

        [Fact]
        public void Serialize_ShouldThrowArgumentNullException_WhenProjectIsNull()
        {
            Action act = () => _serializer.Serialize(null!);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Serialize_ShouldProduceEmptyDocumentsArray_WhenProjectHasNoDocuments()
        {
            var project = new VibProject("EmptyProject", "path/empty.vibproj");

            var json = _serializer.Serialize(project);
            var node = JsonNode.Parse(json);

            node!["documents"]!.AsArray().Should().BeEmpty();
        }

        [Fact]
        public void Serialize_ShouldSkipDocuments_WithNullOrEmptyFilePath()
        {
            var project = new VibProject("Project", "path/p.vibproj");
            project.Documents.Add(new VibDocument("valid.vib", "path/to/valid.vib"));
            project.Documents.Add(new VibDocument("no-path.vib", null!));
            project.Documents.Add(new VibDocument("empty.vib", ""));

            var json = _serializer.Serialize(project);
            var node = JsonNode.Parse(json);

            var docs = node!["documents"]!.AsArray();
            docs.Should().HaveCount(1);
            docs[0]!.GetValue<string>().Should().Be("path/to/valid.vib");
        }

        [Fact]
        public void Serialize_ShouldProducePathsOnly_NotEmbeddedDocumentContent()
        {
            var project = new VibProject("P", "p.vibproj");
            project.Documents.Add(new VibDocument("doc.vib", "docs/doc.vib"));

            var json = _serializer.Serialize(project);
            var node = JsonNode.Parse(json);

            var firstDoc = node!["documents"]!.AsArray()[0];

            firstDoc!.GetValueKind().Should().Be(JsonValueKind.String);
        }

        [Fact]
        public void Deserialize_ShouldCreateProjectWithCorrectMetadata()
        {
            var json = """
            {
                "name": "ImportedProject",
                "version": "2.0",
                "documents": [
                    {
                        "path": "C:/vibas/doc1.vib"
                    },
                    {
                        "path": "C:/vibas/doc2.vib"
                    }
                ]
            }
            """;

            var project = _serializer.Deserialize(json);

            project.Should().NotBeNull();
            project.FileName.Should().Be("ImportedProject.vibproj");
            project.Version.Should().Be("2.0");
            project.Documents.Should().HaveCount(2);
        }

        [Fact]
        public void Deserialize_ShouldSetDocumentFilePaths_FromJsonArray()
        {
            var json = """
            {
                "name": "P",
                "version": "1.0",
                "documents": [
                    {
                        "path": "folder/doc1.vib"
                    }, 
                    {
                        "path": "folder/doc2.vib"
                    }
                ]
            }
            """;

            var project = _serializer.Deserialize(json);

            project.Documents[0].FilePath.Should().Be("folder/doc1.vib");
            project.Documents[1].FilePath.Should().Be("folder/doc2.vib");
        }

        [Fact]
        public void Deserialize_ShouldDeriveDocumentFileName_FromFilePath()
        {
            var json = """
            {
                "name": "P",
                "version": "1.0",
                "documents": [
                    {
                        "path": "folder/subfolder/algo1.vib"
                    }
                ]
            }
            """;

            var project = _serializer.Deserialize(json);

            project.Documents[0].FileName.Should().Be("algo1.vib");
        }

        [Fact]
        public void Deserialize_ShouldReturnEmptyDocumentsList_WhenArrayIsEmpty()
        {
            var json = """
            {
                "name": "P",
                "version": "1.0",
                "documents": []
            }
            """;

            var project = _serializer.Deserialize(json);

            project.Documents.Should().BeEmpty();
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("not-json-at-all")]
        [InlineData("{invalid}")]
        public void Deserialize_ShouldThrow_WhenJsonIsMalformed(string bad)
        {
            Action act = () => _serializer.Deserialize(bad);
            act.Should().ThrowExactly<JsonException>();
        }

        [Fact]
        public void Deserialize_ShouldThrow_WhenRequiredFieldNameIsMissing()
        {
            var json = """{ "version": "1.0", "documents": [] }""";

            Action act = () => _serializer.Deserialize(json);
            act.Should().Throw<Exception>();
        }

        [Fact]
        public void RoundTrip_ShouldPreserveAllFields()
        {
            var original = new VibProject("RoundTripProject", "rt.vibproj")
            {
                Version = "3.1.4"
            };
            original.Documents.Add(new VibDocument("a.vib", "docs/a.vib"));
            original.Documents.Add(new VibDocument("b.vib", "docs/b.vib"));

            var json = _serializer.Serialize(original);
            var restored = _serializer.Deserialize(json);

            restored.FileName.Should().Be(original.FileName);
            restored.Version.Should().Be(original.Version);
            restored.Documents.Should().HaveCount(original.Documents.Count);
            restored.Documents.Select(d => d.FilePath)
                    .Should().BeEquivalentTo(original.Documents.Select(d => d.FilePath));
        }
    }
}