
namespace Basket.Basket.Features.DeleteBasket
{
   // public record DeleteBasketRequest(string UserName);
    public record DeleteBasketResponse(bool Success);
    public class DeleteBasketEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/deletebasket/{UserName}", async (string UserName, ISender sender) =>
            {
               // var command = request.Adapt<DeleteBasketCommand>();
                var result = await sender.Send(new DeleteBasketCommand(UserName));
                var response = result.Adapt<DeleteBasketResponse>();
                return Results.Ok(response);
            })
            .WithName("Deletebasket")
            .Produces<DeleteBasketResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Delete Basket")
            .WithDescription("Delete Basket.");


        }
    }
}
