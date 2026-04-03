using FluentAssertions;
using shared.Projects;

namespace Shared.Tests.Projects
{
    public class SbProjectTests
    {
        [Fact]
        public void Constructor_ShouldSetName()
        {
            var project = new SbProject("Test");

            project.Name.Should().Be("Test");
        }

        [Fact]
        public void HasValidName_ShouldReturnTrue_WhenNameIsNotEmpty()
        {
            var project = new SbProject("Test");

            project.HasValidName().Should().BeTrue();
        }

        [Fact]
        public void HasValiatesValidName_ShouldReturnFalse_WhenNameIsEmpty()
        {
            var project = new SbProject("");

            project.HasValidName().Should().BeFalse();
        }
    }
}
