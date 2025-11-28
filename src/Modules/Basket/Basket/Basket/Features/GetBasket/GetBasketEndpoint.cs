
namespace Basket.Basket.Features.GetBasket
{
    //public record GetBasketRequest(string UserName) : IQuery<GetBasketResponse>;
    public record GetBasketResponse(ShoppingCartDto ShoppingCart);
    public class GetBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/basket/{UserName}", async (string UserName, ISender sender) =>
            {
                var result = await sender.Send(new GetBasketQuery(UserName));
                var response = result.Adapt<GetBasketResponse>();
                return Results.Ok(response);
            })
          .WithName("GetBasket")
          .Produces<GetBasketResponse>(StatusCodes.Status201Created)
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .WithSummary("Basket Details.")
          .WithDescription("Basket Details.")
          .RequireAuthorization();
        }
    }
}
