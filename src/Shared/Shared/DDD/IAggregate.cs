namespace Shared.DDD
{
    public interface IAggregate<T> : IAggregate, IEntity<T>
    {
    }

    public interface IAggregate : IEntity
    {
        IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
        IDomainEvent[] ClearDomainEvents();
    }
}
