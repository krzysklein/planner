namespace Tasks.Application.Dto
{
    public record ErrorResponse(
        string Message,
        object? Details);
}
