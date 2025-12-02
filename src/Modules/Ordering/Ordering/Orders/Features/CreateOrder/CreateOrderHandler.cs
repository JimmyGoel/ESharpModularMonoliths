

namespace Ordering.Orders.Features.CreateOrder
{
    public record CreateOrderCommand(OrderDTo Order)
        : ICommand<CreateOrderResult>;
    public record CreateOrderResult(Guid Id);

    public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderValidator()
        {
            RuleFor(x => x.Order).NotNull();
            RuleFor(x => x.Order.CustomerId).NotEmpty();
            RuleFor(x => x.Order.OrderName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Order.ShippingAddress).NotNull();
            RuleFor(x => x.Order.BillingAddress).NotNull();
            RuleFor(x => x.Order.Payment).NotNull();
            RuleFor(x => x.Order.OrderItems).NotEmpty();
        }
    }
    internal class CreateOrderHandler(OrderDbContext orderDbContext)
        : ICommandHandler<CreateOrderCommand, CreateOrderResult>
    {
        public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var order = CreateNewOrder(command.Order);
            orderDbContext.Orders.Add(order);
            await orderDbContext.SaveChangesAsync(cancellationToken);
            return new CreateOrderResult(order.Id);
        }

        private Order CreateNewOrder(OrderDTo order)
        {
            var _shippingAdderss = Address.Of(
                order.ShippingAddress.FirstName,
                order.ShippingAddress.LastName,
                order.ShippingAddress.EmailAddress,
                order.ShippingAddress.AddressLine,
                order.ShippingAddress.State,
                order.ShippingAddress.Country,
                order.ShippingAddress.ZipCode);
            var _billingAddress = Address.Of(
                order.BillingAddress.FirstName,
                order.BillingAddress.LastName,
                order.BillingAddress.EmailAddress,
                order.BillingAddress.AddressLine,
                order.BillingAddress.State,
                order.BillingAddress.Country,
                order.BillingAddress.ZipCode);
            var _payment = Payment.Of(
                order.Payment.CardName,
                order.Payment.CardNumber,
                order.Payment.Expiration,
                order.Payment.Cvv,
                order.Payment.PaymentMethod);
            var newOrder = Order.Create(
                Guid.NewGuid(),
                order.CustomerId,
                order.OrderName,
                _shippingAdderss,
                _billingAddress,
                _payment);
            order.OrderItems.ForEach(item =>
               {
                   newOrder.Add(item.ProductId, item.Quantity, item.Price);
               });
            return newOrder;
        }
    }
}
