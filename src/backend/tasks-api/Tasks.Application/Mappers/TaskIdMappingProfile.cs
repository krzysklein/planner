using AutoMapper;
using System;
using Tasks.Domain;

namespace Tasks.Application.Mappers
{
    public class TaskIdMappingProfile : Profile
    {
        public TaskIdMappingProfile()
        {
            CreateMap<Guid, TaskId>()
                .ConstructUsing(guid => new TaskId(guid));

            CreateMap<TaskId, Guid>()
                .ConstructUsing(taskId => taskId.Value);
        }
    }
}
