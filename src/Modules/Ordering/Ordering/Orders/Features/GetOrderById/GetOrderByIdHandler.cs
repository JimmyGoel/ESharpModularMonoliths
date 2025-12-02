


using Mapster;

namespace Ordering.Orders.Features.GetOrderById
{
    public record GetOrderByIdQuery(Guid OrderId)
        : IQuery<GetOrderByIdResult>;
    public record GetOrderByIdResult(OrderDTo Order);

    public class GetOrderByIdValidator : AbstractValidator<GetOrderByIdQuery>
    {
        public GetOrderByIdValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty().WithMessage("Order Id Required");
        }
    }
    internal class GetOrderByIdHandler(OrderDbContext orderDbContext)
        : IQueryHandler<GetOrderByIdQuery, GetOrderByIdResult>
    {
        public async Task<GetOrderByIdResult> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await orderDbContext.Orders
                .AsNoTracking()
                .Include(x => x.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);
            if (order == null)
            {
                throw new OrderNotFoundException(request.OrderId);
            }
            var orderDto = order.Adapt<OrderDTo>();
            return new GetOrderByIdResult(orderDto);
        }
    }
}
