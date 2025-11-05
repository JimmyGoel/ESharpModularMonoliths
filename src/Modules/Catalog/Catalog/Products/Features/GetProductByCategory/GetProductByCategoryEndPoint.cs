
namespace Catalog.Products.Features.GetProductByCategory
{
    public record GetProductByCategoryResponse(IEnumerable<ProductDto> Products);
    public class GetProductByCategoryEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/category/{category}", async (string category, ISender sender) =>
            {
                var result = await sender.Send(new GetProductsByCategoryQuery(category));
                var response = result.Adapt<GetProductByCategoryResponse>();
                return Results.Ok(response.Products);
            })
         .WithName("GetProductByCategory")
         .Produces<GetProductByCategoryResponse>(StatusCodes.Status201Created)
         .ProducesProblem(StatusCodes.Status400BadRequest)
         .WithSummary("Product Details Catgory.")
         .WithDescription("Product Details Catgory.");
        }
    }
}
