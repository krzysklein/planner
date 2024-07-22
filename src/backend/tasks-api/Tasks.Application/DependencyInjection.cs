using Microsoft.Extensions.DependencyInjection;
using FluentValidation;

namespace Tasks.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<ITasksService, TasksService>();

            var thisAssembly = typeof(DependencyInjection).Assembly;
            services.AddValidatorsFromAssembly(thisAssembly);
            services.AddAutoMapper(thisAssembly);

            return services;
        }
    }
}
