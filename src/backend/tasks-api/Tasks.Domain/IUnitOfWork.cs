using System.Threading;

namespace Tasks.Domain
{
    public interface IUnitOfWork
    {
        System.Threading.Tasks.Task Begin(CancellationToken cancellationToken);
        System.Threading.Tasks.Task Commit(CancellationToken cancellationToken);
        System.Threading.Tasks.Task Rollback(CancellationToken cancellationToken);
    }
}
