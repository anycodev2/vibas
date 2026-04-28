using FluentAssertions;
using shared.Documents;

namespace Shared.Tests.Documents
{
    public class VibDocumentTests
    {
        [Fact]
        public void Constructor_ShouldSetFileName()
        {
            var document = new VibDocument("Test");
            var document2 = new VibDocument("Test.vib");

            document.FileName.Should().Be("Test.vib");
            document2.FileName.Should().Be("Test.vib");
        }

        [Fact]
        public void Constructor_ShouldSetFilePath()
        {
            var document = new VibDocument("Test.vibdoc", "C:/docs/Test.vibdoc");

            document.FilePath.Should().Be("C:/docs/Test.vibdoc");
        }

        [Fact]
        public void Constructor_ShouldInitializeBlocksList()
        {
            var document = new VibDocument("Test", "path");

            document.Blocks.Should().NotBeNull();
            document.Blocks.Should().BeEmpty();
        }

        [Fact]
        public void Constructor_ShouldInitializeConnectionsList()
        {
            var document = new VibDocument("Test", "path");

            document.Connections.Should().NotBeNull();
            document.Connections.Should().BeEmpty();
        }

        [Fact]
        public void Constructor_ShouldSetIsModifiedToFalse()
        {
            var document = new VibDocument("Test", "path");

            document.isModified.Should().BeFalse();
        }

        // HasValidName tests
        [Fact]
        public void HasValidName_ShouldReturnFalse_WhenNameIsNull()
        {
            var document = new VibDocument();
            document.HasValidName().Should().BeFalse();
        }

        [Fact]
        public void HasValidName_ShouldReturnFalse_WhenNameIsWhitespace()
        {
            var document = new VibDocument("     ");
            document.HasValidName().Should().BeFalse();
        }

        [Fact]
        public void HasValidName_ShouldReturnFalse_WhenNameIsTooLong()
        {
            var fileName = new string('a', 129) + ".vib";
            var document = new VibDocument(fileName);

            document.HasValidName().Should().BeFalse();
        }

        [Theory]
        [InlineData(".")]
        [InlineData("..")]
        [InlineData("te/st")]
        [InlineData("te\\st")]
        [InlineData("te:st")]
        [InlineData("te*st")]
        [InlineData("te?st")]
        [InlineData("te<st")]
        [InlineData("te>st")]
        [InlineData("te|st")]
        [InlineData("te\"st")]
        public void HasValidName_ShouldReturnFalse_WhenNameContainsInvalidCharacters(string fileName)
        {
            var document = new VibDocument(fileName);

            document.HasValidName().Should().BeFalse();
        }

        [Theory]
        [InlineData("CON")]
        [InlineData("PRN")]
        [InlineData("AUX")]
        [InlineData("NUL")]
        [InlineData("COM1")] // 1 to 9 are reserved names in Windows
        [InlineData("COM9")]
        [InlineData("LPT1")] // 1 to 9 are reserved names in Windows
        [InlineData("LPT9")]
        public void HasValidName_ShouldReturnFalse_WhenNameIsReserved(string fileName)
        {
            var document = new VibDocument(fileName);
            document.HasValidName().Should().BeFalse();
        }

        [Fact]
        public void HasValidName_ShouldReturnTrue_WhenNameIsValid()
        {
            var document = new VibDocument("ValidDocument.vib");
            document.HasValidName().Should().BeTrue();
        }
    }
}
