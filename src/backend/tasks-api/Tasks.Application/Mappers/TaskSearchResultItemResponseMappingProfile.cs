using AutoMapper;
using Tasks.Application.Dto;
using Tasks.Domain;

namespace Tasks.Application.Mappers
{
    public class TaskSearchResultItemResponseMappingProfile : Profile
    {
        public TaskSearchResultItemResponseMappingProfile()
        {
            CreateMap<Task, TaskSearchResultItemResponse>();
        }
    }
    public class TaskSearchResultResponseMappingProfile : Profile
    {
        public TaskSearchResultResponseMappingProfile()
        {
            CreateMap<(Task[] Tasks, int TotalCount), TaskSearchResultResponse>()
                .ConstructUsing((x, ctx) => 
                    new TaskSearchResultResponse(x.TotalCount, ctx.Mapper.Map<TaskSearchResultItemResponse[]>(x.Tasks)));
        }
    }
}
