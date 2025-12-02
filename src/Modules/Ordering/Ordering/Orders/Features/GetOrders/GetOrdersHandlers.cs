using Mapster;
using Shared.Pagination;

namespace Ordering.Orders.Features.GetOrders
{
    public record GetOrdersQuery(PaginationRequest PaginationRequest)
        : IQuery<GetOrdersResult>;
    public record GetOrdersResult(PaginatedResult<OrderDTo> Orders);
    internal class GetOrdersHandlers(OrderDbContext orderDbContext)
        : IQueryHandler<GetOrdersQuery, GetOrdersResult>
    {
        public async Task<GetOrdersResult> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var pageIndex = request.PaginationRequest.PageIndex;
            var pageSize = request.PaginationRequest.PageSize;
            var totalCount = await orderDbContext.Orders.LongCountAsync(cancellationToken);

            var orders = await orderDbContext.Orders
                .AsNoTracking()
                .Include(x => x.OrderItems)
                .OrderBy(p => p.OrderName)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var orderDto = orders.Adapt<List<OrderDTo>>();

            return new GetOrdersResult(
                new PaginatedResult<OrderDTo>
                (
                    pageIndex, pageSize, totalCount, orderDto
                    )
                );
        }
    }
}
