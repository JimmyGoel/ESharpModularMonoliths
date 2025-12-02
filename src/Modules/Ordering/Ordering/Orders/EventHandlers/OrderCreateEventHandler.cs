

namespace Ordering.Orders.EventHandlers
{
    public class OrderCreateEventHandler(ILogger<OrderCreateEventHandler> logger)
        : INotificationHandler<OrderCreateEvent>
    {
        public Task Handle(OrderCreateEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);
            return Task.CompletedTask;
        }
    }
}

