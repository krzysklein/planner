using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using Tasks.Domain;

namespace Tasks.Infrastructure.Persistance
{
    public class TaskRepository : ITaskRepository
    {
        private readonly DatabaseContext databaseContext;

        public TaskRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public void AddTask(Task task)
        {
            databaseContext.Tasks.Add(task);
        }

        public void RemoveTask(Task task)
        {
            databaseContext.Tasks.Remove(task);
        }

        public async System.Threading.Tasks.Task<Task?> GetTaskById(TaskId id, CancellationToken cancellationToken)
        {
            return await databaseContext.Tasks.FindAsync([id], cancellationToken);
        }

        public async System.Threading.Tasks.Task<(Task[] Tasks, int TotalCount)> SearchTasks(
            string? searchPhrase,
            TaskState? state,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            IQueryable<Task> query = databaseContext.Tasks;

            if (!string.IsNullOrWhiteSpace(searchPhrase))
            {
                query = query
                    .Where(x => x.Name.Contains(searchPhrase) || x.Description!.Contains(searchPhrase));
            }

            if (state is not null)
            {
                query = query
                    .Where(x => x.State == state);
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var tasks = await query
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .ToArrayAsync(cancellationToken);

            return (tasks, totalCount);
        }
    }
}
