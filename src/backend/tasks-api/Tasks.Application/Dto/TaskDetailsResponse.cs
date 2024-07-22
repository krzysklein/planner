using System;

namespace Tasks.Application.Dto
{
    public record TaskDetailsResponse(
        Guid Id,
        string Name,
        string Description,
        string State);
}
