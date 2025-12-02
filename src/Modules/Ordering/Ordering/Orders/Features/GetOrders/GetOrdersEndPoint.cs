
namespace Ordering.Orders.Features.GetOrders
{
    public record GetOrderResponse(PaginatedResult<OrderDTo> Orders);
    internal class GetOrdersEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/orders", async ([AsParameters] PaginationRequest request, ISender sender) =>
            {
                var result = await sender.Send(new GetOrdersQuery(request));
                GetOrderResponse response = result.Adapt<GetOrderResponse>();
                return Results.Ok(response);

            })
             .WithName("GetOrders")
            .Produces<GetOrderResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithSummary("Get All Orders")
            .WithDescription("Get All orders");

        }
    }
}
