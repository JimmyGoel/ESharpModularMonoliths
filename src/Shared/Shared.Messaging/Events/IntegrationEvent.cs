namespace Shared.Messaging.Events
{
    public record class IntegrationEvent
    {
        public Guid EventId { get; init; } = Guid.NewGuid();
        public DateTime OccuredOn { get; init; } = DateTime.UtcNow;
        public string EventType => GetType().AssemblyQualifiedName!;
    }
}
