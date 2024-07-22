using Bogus;
using FluentAssertions;
using System;
using Tasks.Domain;
using Xunit;

namespace Tasks.Tests
{
    public class TaskCreationTests
    {
        private readonly Faker faker = new Faker();

        [Theory]
        [InlineData("active task 1", "an active task", TaskState.Active)]
        [InlineData("active task 2", "", TaskState.Active)]
        [InlineData("active task 3", null, TaskState.Active)]
        [InlineData("inactive task 1", "", TaskState.Inactive)]
        [InlineData("closed task 1", "", TaskState.Closed)]
        public void should_create_task_when_all_arguments_are_properly_passed(string name, string? description, TaskState state)
        {
            // Arrange

            // Act
            var task = new Task(name, description, state);

            // Assert
            task.Id.Value.Should().NotBeEmpty();
            task.Name.Should().Be(name);
            task.Description.Should().Be(description);
            task.State.Should().Be(state);
        }

        [Fact]
        public void should_throw_argument_null_exception_when_name_is_passed_null()
        {
            // Arrange
            var description = faker.Lorem.Word();
            var state = faker.PickRandom<TaskState>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>("name", () => new Task(null!, description, state));
        }

        [Theory]
        [InlineData("")]
        [InlineData("    ")]
        public void should_throw_argument_exception_when_name_is_passed_empty_or_whitespace(string name)
        {
            // Arrange
            var description = faker.Lorem.Word();
            var state = faker.PickRandom<TaskState>();

            // Act & Assert
            Assert.Throws<ArgumentException>("name", () => new Task(name!, description, state));
        }
    }
}