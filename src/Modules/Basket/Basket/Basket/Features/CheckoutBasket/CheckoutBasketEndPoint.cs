
namespace Basket.Basket.Features.CheckoutBasket
{
    public record CheckOutBasketRequest(BasketCheckoutDTos BasketCheckout);
    public record CheckOutBasketResponse(bool IsSuccess);
    internal class CheckoutBasketEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket/checkout", async (CheckOutBasketRequest request, ISender sender) =>
            {

                var command = request.Adapt<CheckoutBasketCommand>();

                command = command with
                {
                    BasketCheckout = command.BasketCheckout with
                    {
                        cvv = request.BasketCheckout.cvv
                    }
                };

                var result = await sender.Send(command);

                var response = result.Adapt<CheckOutBasketResponse>();

                return Results.Ok(response);
            })
             .WithName("CheckoutBasket")
             .Produces<CheckOutBasketResponse>(StatusCodes.Status201Created)
             .ProducesProblem(StatusCodes.Status400BadRequest)
             .WithSummary("Checkout Basket Created")
             .WithDescription("Checkout Basket Creted")
             .RequireAuthorization();
            
        }
    }
}
