using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using Tasks.Domain;

namespace Tasks.Infrastructure.Persistance.Configurations
{
    public class TaskIdConverter : ValueConverter<TaskId, Guid>
    {
        public TaskIdConverter()
            : base(x => x.Value, x => new TaskId(x))
        {
        }
    }
}
