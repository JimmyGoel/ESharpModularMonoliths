
namespace Basket.Basket.Features.AddItemIntoBasket
{
    public record AddItemIntoBasketRequest(string userName, ShoppingCartItemDto ShoppingCartItem);
    public record AddItemIntoBasketResponse(Guid Id);
    public class AddItemIntoBasketEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket/{userName}/items", async (
                [FromRoute] string userName,
                [FromBody] AddItemIntoBasketRequest request,
                ISender sender) =>
            {
                var command = new AddItemIntoBasketCommand(userName, request.ShoppingCartItem);
                var result = await sender.Send(command);
                var response = result.Adapt<AddItemIntoBasketResponse>();
                return Results.Created($"/basket/{response.Id}", response);
            })
           .WithName("Createbasket")
           .Produces<AddItemIntoBasketResponse>(StatusCodes.Status201Created)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .WithSummary("Create Basket")
           .WithDescription("Creates Basket with the provided details.");
        }
    }
}
