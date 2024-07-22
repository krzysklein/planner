namespace Tasks.Application.Dto
{
    public record TaskSearchResultResponse(
        int TotalCount,
        TaskSearchResultItemResponse[] Items);
}
