using Basket.Basket.Features.UpdateItemPriceBasket;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Messaging.Events;

namespace Basket.Basket.EventHandlers
{
    public class ProductPriceChangedIntegrationEventHandler
        (ISender sender, ILogger<ProductPriceChangedIntegrationEventHandler> logger) :
        IConsumer<ProductPriceChangeIntegrationEvent>
    {
        public async Task Consume(ConsumeContext<ProductPriceChangeIntegrationEvent> context)
        {
            logger.LogInformation("Integration Event handler : {IntegrationEvent}", context.Message.GetType().Name);

            var command = new UpdateItemPriceInBasketComand(NewPrice: context.Message.Price, ProductId: context.Message.ProductId);
            var result = await sender.Send(command);

            if (!result.IsPriceUpdated)
            {
                logger.LogInformation("Basket items updated for ProductId: {ProductId} with New Price: {NewPrice}", context.Message.ProductId, context.Message.Price);
            }
            else
            {
                logger.LogWarning("No basket items found for ProductId: {ProductId} to update price.", context.Message.ProductId);
            }

        }
    }
}
