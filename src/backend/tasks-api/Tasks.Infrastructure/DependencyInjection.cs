using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tasks.Domain;
using Tasks.Infrastructure.Persistance;

namespace Tasks.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddScoped<ITaskRepository, TaskRepository>();

            services.AddDbContext<IUnitOfWork, DatabaseContext>(options => options
                .UseSqlServer(configuration.GetConnectionString("DatabaseContext")));

            return services;
        }
    }
}
