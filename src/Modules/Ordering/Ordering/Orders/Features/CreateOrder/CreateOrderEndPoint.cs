

namespace Ordering.Orders.Features.CreateOrder
{
    public record CreateOrderRequest(OrderDTo Order);
    public record CreateOrderResponse(Guid Id);
    internal class CreateOrderEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/orders", async (CreateOrderRequest request, ISender sender) =>
            {

                var command = request.Adapt<CreateOrderCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<CreateOrderResponse>();

                return Results.Created($"/Orders/{response.Id}", response);
            })
            .WithName("CreateOrder")
            .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Order Created")
            .WithDescription("Order Creted");
        }
    }
}
