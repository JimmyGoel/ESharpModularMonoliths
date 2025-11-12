
namespace Basket.Basket.Features.CreateBasket
{
    public record CreateBasketRequest(ShoppingCartDto ShoppingCart);
    public record CreateBasketRespons(Guid Id);
    public class CreateBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket", async (CreateBasketRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateBasketCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<CreateBasketRespons>();
                return Results.Created($"/basket/{response.Id}", response);
            })
            .WithName("Createbasket")
            .Produces<CreateBasketRespons>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Basket")
            .WithDescription("Creates Basket with the provided details.");


        }
    }
}
