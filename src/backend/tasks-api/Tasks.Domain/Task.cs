using EnsureThat;

namespace Tasks.Domain
{
    public class Task: IAggregateRoot<TaskId>
    {
        public TaskId Id { get; init; }
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public TaskState State { get; private set; }

        public Task(string name, string? description, TaskState state)
        {
            Ensure.That(name, nameof(name))
                .IsNotNullOrWhiteSpace();

            Id = TaskId.CreateNew();
            Name = name;
            Description = description;
            State = state;
        }

        public void ChangeDetails(string name, string? description)
        {
            Ensure.That(name, nameof(name))
                .IsNotEmptyOrWhiteSpace();

            Name = name;
            Description = description;
        }

        public void ChangeState(TaskState state)
        {
            State = state;
        }
    }
}
