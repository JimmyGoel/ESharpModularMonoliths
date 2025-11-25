
using MassTransit;
using Shared.Messaging.Events;

namespace Catalog.Products.EventHadlers
{
    public class ProductPriceChangeEventHandler
        (IBus bus, ILogger<ProductPriceChangeEventHandler> _logger)
        : INotificationHandler<ProductPriceChangeEvent>
    {
        public async Task Handle(ProductPriceChangeEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Domain Price Change Event handler : {DomainEvent}", notification.GetType().Name);
            //return Task.CompletedTask;
            var integrationEvent = new ProductPriceChangeIntegrationEvent
            {
                ProductId = notification.Product.Id,
                Name = notification.Product.Name,
                Category = notification.Product.Category,
                Description = notification.Product.Description,
                ImageFile = notification.Product.ImageFile,
                Price = notification.Product.Price,
            };

            await bus.Publish(integrationEvent, cancellationToken);
        }
    }
}
