
namespace Catalog.Products.EventHadlers
{
    public class ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> _logger)
        : INotificationHandler<ProductCreatedEvent>
    {
        public Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Domain Event handler : {DomainEvent}", notification.GetType().Name);
            return Task.CompletedTask;
        }
    }
}
