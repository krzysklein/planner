using Bogus;
using Tasks.Application.Dto;

namespace Tasks.IntegrationTests.Fakers
{
    public static class CreateTaskRequestFaker
    {
        private static readonly string[] taskStatuses = ["inactive", "active", "closed"];

        public static readonly Faker<CreateTaskRequest> Default = new Faker<CreateTaskRequest>()
            .CustomInstantiator(f => new CreateTaskRequest(
                f.Lorem.Word(),
                f.Lorem.Word(),
                f.PickRandom(taskStatuses).ToString()));

        public static readonly Faker<CreateTaskRequest> WithNullName = new Faker<CreateTaskRequest>()
            .CustomInstantiator(f => new CreateTaskRequest(
                null!,
                f.Lorem.Word(),
                f.PickRandom(taskStatuses).ToString()));
    }
}
