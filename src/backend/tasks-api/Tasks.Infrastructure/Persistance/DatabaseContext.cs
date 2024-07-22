using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading;
using Tasks.Domain;

namespace Tasks.Infrastructure.Persistance
{
    public class DatabaseContext : DbContext, IUnitOfWork
    {
        public DbSet<Task> Tasks { get; set; }
        private IDbContextTransaction? currentTransaction = null;

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public async System.Threading.Tasks.Task Begin(CancellationToken cancellationToken)
        {
            if (currentTransaction is not null)
            {
                await currentTransaction.RollbackAsync(cancellationToken);
            }

            currentTransaction = await Database.BeginTransactionAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task Commit(CancellationToken cancellationToken)
        {
            await SaveChangesAsync(cancellationToken);

            if (currentTransaction is not null)
            {
                await currentTransaction.CommitAsync(cancellationToken);
                currentTransaction = null;
            }
        }

        public async System.Threading.Tasks.Task Rollback(CancellationToken cancellationToken)
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                }
            }

            if (currentTransaction is not null)
            {
                await currentTransaction.RollbackAsync(cancellationToken);
                currentTransaction = null;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
        }
    }
}
