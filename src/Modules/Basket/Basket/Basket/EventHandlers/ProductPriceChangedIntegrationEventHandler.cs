using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Messaging.Events;

namespace Basket.Basket.EventHandlers
{
    public class ProductPriceChangedIntegrationEventHandler
        (ISender sender, ILogger<ProductPriceChangedIntegrationEventHandler> logger) :
        IConsumer<ProductPriceChangeIntegrationEvent>
    {
        public Task Consume(ConsumeContext<ProductPriceChangeIntegrationEvent> context)
        {
            logger.LogInformation("Integration Event handler : {IntegrationEvent}", context.Message.GetType().Name);
            return Task.CompletedTask;
        }
    }
}
