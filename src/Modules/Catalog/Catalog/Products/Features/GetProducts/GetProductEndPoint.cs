
namespace Catalog.Products.Features.GetProducts
{
    public record GetProductResponse(IEnumerable<ProductDto> Product);
    public class GetProductEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async (ISender sender) =>
            {
                var result = await sender.Send(new GetProductsQuery());
                var response = result.Adapt<GetProductResponse>();
                return Results.Ok(response.Product);
            })
          .WithName("GetProduct")
          .Produces<GetProductResponse>(StatusCodes.Status201Created)
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .WithSummary("Product Details.")
          .WithDescription("Product Details.");
        }
    }
}
