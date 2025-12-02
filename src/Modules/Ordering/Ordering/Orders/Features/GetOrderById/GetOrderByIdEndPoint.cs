
namespace Ordering.Orders.Features.GetOrderById
{
    //public record GetOrderByIdRequest(Guid Id);
    public record GetOrderByIdResponse(OrderDTo Order);
    internal class GetOrderByIdEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/orders/{id}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new GetOrderByIdQuery(id));
                var response = result.Adapt<GetOrderByIdResponse>();
                return Results.Ok(response);
            })
           .WithName("GetOrderById")
          .Produces<GetOrderByIdResponse>(StatusCodes.Status201Created)
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .WithSummary("Product Id Details.")
          .WithDescription("Product ID Details.");
        }
    }
}
