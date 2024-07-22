using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Tasks.Api;
using Tasks.Domain;
using Tasks.Infrastructure.Persistance;
using Testcontainers.MsSql;

namespace Tasks.IntegrationTests
{
    public class IntegrationTestsWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly MsSqlContainer msSqlContainer = 
            new MsSqlBuilder()
                .Build();

        public async System.Threading.Tasks.Task InitializeAsync()
        {
            await msSqlContainer.StartAsync();

            using var scope = Services.CreateScope();
            var databaseContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            await databaseContext.Database.MigrateAsync();
        }

        async System.Threading.Tasks.Task IAsyncLifetime.DisposeAsync()
        {
            await msSqlContainer.StopAsync();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<DatabaseContext>));
                services.AddDbContext<IUnitOfWork, DatabaseContext>(options => options
                    .UseSqlServer(msSqlContainer.GetConnectionString()));
            });
        }
    }
}