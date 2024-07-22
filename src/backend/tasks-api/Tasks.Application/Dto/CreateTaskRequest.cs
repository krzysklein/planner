namespace Tasks.Application.Dto
{
    public record CreateTaskRequest(
        string Name,
        string Description,
        string State);
}
