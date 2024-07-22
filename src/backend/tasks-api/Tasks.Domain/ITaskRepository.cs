using System.Threading;

namespace Tasks.Domain
{
    public interface ITaskRepository
    {
        void AddTask(Task task);
        void RemoveTask(Task task);
        System.Threading.Tasks.Task<Task?> GetTaskById(TaskId id, CancellationToken cancellationToken);
        System.Threading.Tasks.Task<(Task[] Tasks, int TotalCount)> SearchTasks(
            string? searchPhrase,
            TaskState? state,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);
    }
}
