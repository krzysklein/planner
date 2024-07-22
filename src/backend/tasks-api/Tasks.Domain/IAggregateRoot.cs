namespace Tasks.Domain
{
    public interface IAggregateRoot<TId>
    {
        TId Id { get; }
    }
}
