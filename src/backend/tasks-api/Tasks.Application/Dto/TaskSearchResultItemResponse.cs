using System;

namespace Tasks.Application.Dto
{
    public record TaskSearchResultItemResponse(
        Guid Id,
        string Name,
        string State);
}
