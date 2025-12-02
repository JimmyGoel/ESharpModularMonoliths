


namespace Ordering.Orders.Features.DeleteOrder
{
    public record DeleteOrderCommand(Guid OrderId)
        : ICommand<DeleteOrderResult>;
    public record DeleteOrderResult(bool Success);

    public class DeleteOrderValidator : AbstractValidator<DeleteOrderCommand>
    {
        public DeleteOrderValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty().WithMessage("Order Id Required");
        }
    }
    internal class DeleteOrderHandler(OrderDbContext orderDbContext)
        : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
    {
        public async Task<DeleteOrderResult> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
        {
            var order = await orderDbContext.Orders
                .FindAsync([command.OrderId], cancellationToken: cancellationToken);
            if (order == null)
            {
                throw new OrderNotFoundException(command.OrderId);
            }
            orderDbContext.Orders.Remove(order);
            await orderDbContext.SaveChangesAsync(cancellationToken);
            return new DeleteOrderResult(true);
        }
    }
}
