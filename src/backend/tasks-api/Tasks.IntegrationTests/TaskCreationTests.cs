using FluentAssertions;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using Tasks.Application.Dto;
using Tasks.IntegrationTests.Fakers;

namespace Tasks.IntegrationTests
{
    public class TaskCreationTests : BaseIntegrationTest
    {
        public TaskCreationTests(IntegrationTestsWebApplicationFactory integrationTestsWebApplicationFactory) 
            : base(integrationTestsWebApplicationFactory)
        {
        }

        [Fact]
        public async System.Threading.Tasks.Task should_create_task_when_all_arguments_are_properly_passed()
        {
            // Arrange
            var client = CreateHttpClient();
            var request = CreateTaskRequestFaker.Default.Generate();

            // Act
            var response = await client.PostAsync("tasks", JsonContent.Create(request));

            // Assert
            response.EnsureSuccessStatusCode();
            var taskDetailsResponse = await response.Content.ReadFromJsonAsync<TaskDetailsResponse>();
            taskDetailsResponse.Should().NotBeNull();
            taskDetailsResponse!.Id.Should().NotBeEmpty();
            taskDetailsResponse!.Name.Should().Be(request.Name);
            taskDetailsResponse!.Description.Should().Be(request.Description);
        }

        [Fact]
        public async System.Threading.Tasks.Task should_fail_with_bad_request_when_name_is_not_passed()
        {
            // Arrange
            var client = CreateHttpClient();
            var request = CreateTaskRequestFaker.WithNullName.Generate();

            // Act
            var response = await client.PostAsync("tasks", JsonContent.Create(request));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}