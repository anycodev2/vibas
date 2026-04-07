using FluentAssertions;
using Moq;
using shared.Documents;
using shared.Projects;
using shared.Serialization;
using shared.Services;

namespace Shared.Tests.Services
{
    public class VibProjectServiceTests : IDisposable
    {
        private readonly Mock<IVibSerializer> _serializerMock;
        private readonly Mock<VibDocumentService> _docServiceMock;
        private readonly VibProjectService _service;
        private readonly List<string> _tempFiles;

        public VibProjectServiceTests()
        {
            _serializerMock = new Mock<IVibSerializer>();
            _docServiceMock = new Mock<VibDocumentService>();

            _service = new VibProjectService(_serializerMock.Object, _docServiceMock.Object);
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
        public void Open_ShouldLoadProjectAndSetFilePath()
        {
            // Arrange
            var json = "{\"name\": \"MyProject\"}";
            var path = CreateTempFile(json);
            var expectedProject = new VibProject("MyProject", path);

            _serializerMock
                .Setup(s => s.Deserialize<VibProject>(json))
                .Returns(expectedProject);

            // Act
            var result = _service.Open(path);

            // Assert
            result.Should().NotBeNull();
            result.FilePath.Should().Be(path);
            _serializerMock.Verify(s => s.Deserialize<VibProject>(json), Times.Once);
        }

        [Fact]
        public void Open_ShouldThrowFileNotFound_WhenPathIsInvalid()
        {
            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => _service.Open("non_existent_file.vibproj"));
        }

        // --- Save() ---

        [Fact]
        public void Save_ShouldSerializeAndWriteToFile()
        {
            // Arrange
            var path = Path.Combine(Path.GetTempPath(), "save_test.vibproj");
            _tempFiles.Add(path);
            var project = new VibProject("SaveTest", path);
            var json = "{\"name\": \"SaveTest\"}";

            _serializerMock.Setup(s => s.Serialize(project)).Returns(json);

            // Act
            _service.Save(project);

            // Assert
            File.Exists(path).Should().BeTrue();
            File.ReadAllText(path).Should().Be(json);
        }

        // --- Close() ---

        [Fact]
        public void Close_ShouldNotThrow_WhenProjectIsValid()
        {
            // Arrange
            var project = new VibProject("Test", "path");

            // Act & Assert
            var act = () => _service.Close(project);
            act.Should().NotThrow();
        }

        // --- AddDocument() ---

        [Fact]
        public void AddDocument_ShouldAppendDocumentToProjectList()
        {
            // Arrange
            var project = new VibProject("Project", "path");
            var document = new VibDocument("NewDoc");

            // Act
            _service.AddDocument(project, document);

            // Assert
            project.Documents.Should().Contain(document);
        }

        [Fact]
        public void AddDocument_ShouldThrowException_WhenDocumentIsNull()
        {
            var project = new VibProject("Project", "path");
            Assert.Throws<ArgumentNullException>(() => _service.AddDocument(project, null!));
        }

        // --- RemoveDocument() ---

        [Fact]
        public void RemoveDocument_ShouldRemoveExistingDocument()
        {
            // Arrange
            var document = new VibDocument("ToRemove");
            var project = new VibProject("Project", "path");
            _service.AddDocument(project, document);

            // Act
            _service.RemoveDocument(project, document);

            // Assert
            project.Documents.Should().NotContain(document);
        }

        // --- GetDocument() ---

        [Fact]
        public void GetDocument_ShouldReturnCorrectDocument_ById()
        {
            // Arrange
            var project = new VibProject("Project", "path");
            var document = new VibDocument("FindMe");
            var docId = document.Identifier;

            _service.AddDocument(project, document);

            // Act
            var result = _service.GetDocument(project, docId);

            // Assert
            result.Should().Be(document);
        }

        [Fact]
        public void GetDocument_ShouldThrowKeyNotFound_WhenIdDoesNotExist()
        {
            // Arrange
            var project = new VibProject("Project", "path");

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _service.GetDocument(project, Guid.NewGuid()));
        }
    }
}