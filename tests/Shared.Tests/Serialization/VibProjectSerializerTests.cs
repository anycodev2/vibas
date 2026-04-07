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
        private readonly VibProjectSerializer _serializer;

        public VibProjectSerializerTests()
        {
            _serializer = new VibProjectSerializer();
        }

        // --- Serialization Tests ---

        [Fact]
        public void Serializer_ShouldReturnValidJson_WithMetadataAndDocumentPaths()
        {
            // Arrange
            var project = new VibProject("MyProject", "C:/projects/test.vibproj")
            {
                Version = "1.2.3"
            };

            var doc1 = new VibDocument("algo1.vib", "C:/projects/algorithms/algo1.vib");
            var doc2 = new VibDocument("algo2.vib", "C:/projects/algorithms/algo2.vib");

            project.Documents.Add(doc1);
            project.Documents.Add(doc2);

            // Act
            var jsonString = _serializer.Serializer(project);
            var jsonNode = JsonNode.Parse(jsonString);

            // Assert
            jsonNode.Should().NotBeNull();
            jsonNode!["name"]?.GetValue<string>().Should().Be("MyProject");
            jsonNode["version"]?.GetValue<string>().Should().Be("1.2.3");

            var docsArray = jsonNode["documents"]?.AsArray();
            docsArray.Should().NotBeNull();
            docsArray!.Count.Should().Be(2);

            docsArray.Select(x => x!.GetValue<string>()).Should().Contain(new[]
            {
                "C:/projects/algorithms/algo1.vib",
                "C:/projects/algorithms/algo2.vib"
            });
        }

        [Fact]
        public void Serializer_ShouldThrowArgumentNullException_WhenProjectIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _serializer.Serializer(null!));
        }

        // --- Deserialization Tests ---

        [Fact]
        public void DeSerialize_ShouldCreateProjectWithCorrectMetadata()
        {
            // Arrange plik.vibproj
            var json = @"
            {
                ""name"": ""ImportedProject"",
                ""version"": ""2.0"",
                ""documents"": [
                    ""folder/doc1.vib"",
                    ""folder/doc2.vib""
                ]
            }";

            // Act
            var project = _serializer.DeSerialize(json);

            // Assert
            project.Should().NotBeNull();
            project.FileName.Should().Be("ImportedProject");
            project.Version.Should().Be("2.0");
            project.Documents.Should().HaveCount(2);

            project.Documents[0].FilePath.Should().Be("folder/doc1.vib");
            project.Documents[1].FilePath.Should().Be("folder/doc2.vib");
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("invalid-json")]
        public void DeSerialize_ShouldThrowJsonException_WhenJsonIsMalformed(string invalidJson)
        {
            // Act & Assert
            Assert.ThrowsAny<JsonException>(() => _serializer.DeSerialize(invalidJson));
        }

        // --- Edge Cases ---

        [Fact]
        public void Serializer_ShouldHandleProjectWithNoDocuments()
        {
            // Arrange
            var project = new VibProject("EmptyProject", "path");

            // Act
            var jsonString = _serializer.Serializer(project);
            var jsonNode = JsonNode.Parse(jsonString);

            // Assert
            jsonNode!["documents"]?.AsArray().Should().BeEmpty();
        }

        [Fact]
        public void Serializer_ShouldIgnoreDocumentsWithoutFilePath()
        {
            // Arrange
            var project = new VibProject("Project", "path");
            var validDoc = new VibDocument("valid", "path/to/doc.vib");
            var invalidDoc = new VibDocument("invalid", null);

            project.Documents.Add(validDoc);
            project.Documents.Add(invalidDoc);

            // Act
            var jsonString = _serializer.Serializer(project);
            var jsonNode = JsonNode.Parse(jsonString);

            // Assert
            var docsArray = jsonNode!["documents"]?.AsArray();
            docsArray!.Count.Should().Be(1);
            docsArray[0]!.GetValue<string>().Should().Be("path/to/doc.vib");
        }
    }
}