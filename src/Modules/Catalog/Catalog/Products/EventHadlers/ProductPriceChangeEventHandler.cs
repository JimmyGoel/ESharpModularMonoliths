
namespace Catalog.Products.EventHadlers
{
    public class ProductPriceChangeEventHandler(ILogger<ProductPriceChangeEventHandler> _logger)
        : INotificationHandler<ProductPriceChangeEvent>
    {
        public Task Handle(ProductPriceChangeEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Domain Price Change Event handler : {DomainEvent}", notification.GetType().Name);
            return Task.CompletedTask;
        }
    }
}
