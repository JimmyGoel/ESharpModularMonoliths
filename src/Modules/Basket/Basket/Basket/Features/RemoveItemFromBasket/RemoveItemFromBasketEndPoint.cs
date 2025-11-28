
namespace Basket.Basket.Features.RemoveItemFromBasket
{
    //public record RemoveItemFromBasketRequest(string userName, Guid ProductId);
    public record RemoveItemFromBasketResponse(Guid Id);
    public class RemoveItemFromBasketEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {

            app.MapDelete("/basket/{userName}/items/{itemId}",
                async (
                  [FromRoute] string userName,
                  [FromRoute] Guid itemId,
                  ISender sender) =>
            {
                var command = new RemoveItemFromBasketCommand(userName, itemId);
                var result = await sender.Send(command);
                var response = result.Adapt<RemoveItemFromBasketResponse>();
                return Results.Ok(response);
            })
            .WithName("RemoveItembasket")
            .Produces<RemoveItemFromBasketResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Remove Item Basket")
            .WithDescription("Remove Item Basket.")
            .RequireAuthorization();
        }

    }
}
