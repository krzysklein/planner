using System;

namespace Tasks.Domain
{
    public record TaskId(Guid Value)
    {
        public static TaskId CreateNew()
        {
            return new TaskId(Guid.NewGuid());
        }
    }
}
