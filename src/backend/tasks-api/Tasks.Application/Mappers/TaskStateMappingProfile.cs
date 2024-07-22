using AutoMapper;
using System;
using Tasks.Domain;

namespace Tasks.Application.Mappers
{
    public class TaskStateMappingProfile : Profile
    {
        public TaskStateMappingProfile()
        {
            CreateMap<string, TaskState>()
                .ConstructUsing(state => Enum.Parse<TaskState>(state, true));

            CreateMap<TaskState, string>()
                .ConstructUsing(taskState => taskState.ToString());
        }
    }
}
