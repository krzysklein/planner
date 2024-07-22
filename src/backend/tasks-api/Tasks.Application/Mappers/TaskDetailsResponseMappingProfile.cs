using AutoMapper;
using Tasks.Application.Dto;
using Tasks.Domain;

namespace Tasks.Application.Mappers
{
    public class TaskDetailsResponseMappingProfile : Profile
    {
        public TaskDetailsResponseMappingProfile()
        {
            CreateMap<Task, TaskDetailsResponse>();
        }
    }
}
