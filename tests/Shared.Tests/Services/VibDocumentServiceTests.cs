using FluentAssertions;
using Moq;
using shared.Blocks;
using shared.Blocks.Types;
using shared.Documents;
using shared.Serialization;
using shared.Services;
namespace Shared.Tests.Services
{
    public class VibDocumentServiceTests : IDisposable
    {
        private readonly Mock<IVibSerializer<VibDocument>> _serializerMock;
        private readonly VibDocumentService _service;
        private readonly List<string> _tempFiles;

        public VibDocumentServiceTests()
        {
            _serializerMock = new Mock<IVibSerializer<VibDocument>>();
            _service = new VibDocumentService(_serializerMock.Object);
            _tempFiles = new List<string>();
        }

        private string CreateTempFile(string content = "")
        {
            var path = Path.GetTempFileName();
            File.WriteAllText(path, content);
            _tempFiles.Add(path);
            return path;
        }

        public void Dispose()
        {
            foreach (var file in _tempFiles)
            {
                if (File.Exists(file)) File.Delete(file);
            }
        }

        // --- Open() ---

        [Fact]
        public void Open_ShouldLoadDocumentAndSetFilePath()
        {
            // Arrange
            var json = "{\"FileName\": \"test.vib\"}";
            var path = CreateTempFile(json);
            var expectedDocument = new VibDocument("test.vib", path);

            _serializerMock
                .Setup(s => s.Deserialize(json))
                .Returns(expectedDocument);

            // Act
            var result = _service.Open(path);

            // Assert
            result.Should().NotBeNull();
            result.FilePath.Should().Be(path);
            _serializerMock.Verify(s => s.Deserialize(json), Times.Once);
        }

        [Fact]
        public void Open_ShouldThrowFileNotFound_WhenPathIsInvalid()
        {
            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => _service.Open("missing_file.vib"));
        }

        // --- Save() ---

        [Fact]
        public void Save_ShouldSerializeAndWriteToFile()
        {
            // Arrange
            var path = Path.Combine(Path.GetTempPath(), "save_doc.vib");
            _tempFiles.Add(path);
            var document = new VibDocument("SaveDoc", path);
            var json = "{\"FileName\": \"SaveDoc.vib\"}";

            _serializerMock.Setup(s => s.Serialize(document)).Returns(json);

            // Act
            _service.Save(document);

            // Assert
            File.Exists(path).Should().BeTrue();
            File.ReadAllText(path).Should().Be(json);
        }

        // --- Close() ---

        [Fact]
        public void Close_ShouldNotThrow_WhenDocumentIsValid()
        {
            // Arrange
            var document = new VibDocument("Test", "path");

            // Act & Assert
            var act = () => _service.Close(document);
            act.Should().NotThrow();
        }

        // --- Block Operations ---

        [Fact]
        public void AddBlock_ShouldAppendStartBlockToDocument()
        {
            // Arrange
            var document = new VibDocument("Doc", "path");
            var block = new StartBlock();

            // Act
            _service.AddBlock(document, block);

            // Assert
            document.Blocks.Should().Contain(block);
            document.Blocks.First().Type.Should().Be(BlockType.Start);
        }

        [Fact]
        public void AddBlock_ShouldAppendStatementBlockWithCode()
        {
            // Arrange
            var document = new VibDocument("Doc", "path");
            var block = new StatementBlock { Code = "x = 10;" };

            // Act
            _service.AddBlock(document, block);

            // Assert
            var addedBlock = document.Blocks.OfType<StatementBlock>().FirstOrDefault();
            addedBlock.Should().NotBeNull();
            addedBlock!.Code.Should().Be("x = 10;");
        }

        [Fact]
        public void RemoveBlock_ShouldRemoveExistingBlock()
        {
            // Arrange
            var block = new StartBlock();
            var document = new VibDocument("Doc", "path");
            _service.AddBlock(document, block);

            // Act
            _service.RemoveBlock(document, block);

            // Assert
            document.Blocks.Should().NotContain(block);
        }

        [Fact]
        public void GetBlock_ShouldReturnCorrectBlock_ByIdentifier()
        {
            // Arrange
            var document = new VibDocument("Doc", "path");
            var block = new StatementBlock();
            var blockId = block.Identifier;

            _service.AddBlock(document, block);

            // Act
            var result = _service.GetBlock(document, blockId);

            // Assert
            result.Should().Be(block);
            result.Identifier.Should().Be(blockId);
        }

        [Fact]
        public void GetBlock_ShouldThrowKeyNotFound_WhenIdDoesNotExist()
        {
            // Arrange
            var document = new VibDocument("Doc", "path");

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _service.GetBlock(document, Guid.NewGuid()));
        }

        // --- Connection Operations ---

        [Fact]
        public void AddConnection_ShouldLinkStartAndStatementBlocks()
        {
            // Arrange
            var document = new VibDocument("Doc", "path");
            var start = new StartBlock();
            var statement = new StatementBlock();
            var connection = new VibConnection(start, statement)
            {
                Type = VibConnectionType.Unconditional
            };

            // Act
            _service.AddConnection(document, connection);

            // Assert
            document.Connections.Should().Contain(connection);
            connection.Source.Should().Be(start.Identifier);
            connection.Destination.Should().Be(statement.Identifier);
        }

        [Fact]
        public void RemoveConnection_ShouldRemoveExistingConnection()
        {
            // Arrange
            var start = new StartBlock();
            var statement = new StatementBlock();
            var connection = new VibConnection(start, statement);
            var document = new VibDocument("Doc", "path");

            _service.AddConnection(document, connection);

            // Act
            _service.RemoveConnection(document, connection);

            // Assert
            document.Connections.Should().NotContain(connection);
        }
    }
}