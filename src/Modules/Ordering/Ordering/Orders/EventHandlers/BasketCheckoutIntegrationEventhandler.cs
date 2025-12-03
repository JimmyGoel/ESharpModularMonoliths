using MassTransit;
using Ordering.Orders.DTOs;
using Ordering.Orders.Features.CreateOrder;
using Shared.Messaging.Events;

namespace Ordering.Orders.EventHandlers
{
    public class BasketCheckoutIntegrationEventhandler
        (ISender sender, ILogger<BasketCheckoutIntegrationEventhandler> logger)
        : IConsumer<BasketCheckoutIntegrationEvents>
    {
        public async Task Consume(ConsumeContext<BasketCheckoutIntegrationEvents> context)
        {
            logger.LogInformation("Integration Event handled: {IntegrationEvent} ", context.Message.GetType().Name);
            var CreateOrderCommand = MapToCreateOrderCommand(context.Message);
            var result = await sender.Send(CreateOrderCommand);
        }

        private CreateOrderCommand MapToCreateOrderCommand(BasketCheckoutIntegrationEvents message)
        {
            var addressDto = new AddressDTo
            (
                FirstName: message.FirstName,
                LastName: message.LastName,
                EmailAddress: message.EmailAddress,
                AddressLine: message.AddressLine,
                State: message.State,
                Country: message.Country,
                ZipCode: message.ZipCode
            );

            var payementDto = new PaymentDto
            (
                CardName: message.CardName!,
                CardNumber: message.CardNumber!,
                Expiration: message.Expiration!,
                Cvv: message.CVV!,
                PaymentMethod: message.PaymentMethod
            );

            var orderId = Guid.NewGuid();

            var orderDto = new OrderDTo
            (
                Id: orderId,
                CustomerId: message.CustomerId,
                OrderName: message.UserName,
                ShippingAddress: addressDto,
                BillingAddress: addressDto,
                Payment: payementDto,
                OrderItems:
                [
                    new OrderItemDto
                    (
                        OrderId : orderId,
                        ProductId : Guid.NewGuid(),
                        Quantity : 1,
                        Price : message.TotalPrice
                    ),
                      new OrderItemDto
                    (
                        OrderId : orderId,
                        ProductId : Guid.NewGuid(),
                        Quantity : 2,
                        Price : message.TotalPrice
                    )

                ]
            );
            return new CreateOrderCommand(orderDto);
        }
    }
}
