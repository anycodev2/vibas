using FluentAssertions;
using shared.Projects;

namespace Shared.Tests.Projects
{
    public class VibProjectTests
    {
        [Fact]
        public void Constructor_ShouldSetFileName()
        {
            var project = new VibProject("Test");
            var project2 = new VibProject("Test.vibproj");

            project.FileName.Should().Be("Test.vibproj");
            project.FilePath.Should().BeNull();

            project2.FileName.Should().Be("Test.vibproj");
        }

        [Fact]
        public void Constructor_ShouldSetFilePath()
        {
            var project = new VibProject("Test", "C:/Projects/Test.vibproj");

            project.FilePath.Should().Be("C:/Projects/Test.vibproj");
        }

        [Fact]
        public void Constructor_ShouldInitializeDocumentsList()
        {
            var project = new VibProject("Test.vibproj");

            project.Documents.Should().NotBeNull();
            project.Documents.Should().BeEmpty();
        }

        // HasValidName tests
        [Fact]
        public void HasValidName_ShouldReturnFalse_WhenNameIsNull()
        {
            var project = new VibProject();
            project.HasValidName().Should().BeFalse();
        }

        [Fact]
        public void HasValidName_ShouldReturnFalse_WhenNameIsWhitespace()
        {
            var project = new VibProject("     ");
            project.HasValidName().Should().BeFalse();
        }

        [Fact]
        public void HasValidName_ShouldReturnFalse_WhenNameIsTooLong()
        {
            var fileName = new string('a', 129) + ".vibproj";
            var project = new VibProject(fileName);

            project.HasValidName().Should().BeFalse();
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
            var project = new VibProject(fileName);

            project.HasValidName().Should().BeFalse();
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
            var project = new VibProject(fileName);
            project.HasValidName().Should().BeFalse();
        }

        [Fact]
        public void HasValidName_ShouldReturnTrue_WhenNameIsValid()
        {
            var project = new VibProject("ValidProject.vibproj");
            project.HasValidName().Should().BeTrue();
        }
    }
}
