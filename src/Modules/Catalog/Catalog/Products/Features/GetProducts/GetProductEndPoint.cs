


namespace Catalog.Products.Features.GetProducts
{
    public record GetProductResponse(PaginatedResult<ProductDto> Products);
    public class GetProductEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async ([AsParameters] PaginationRequest request  ,ISender sender) =>
            {
                var result = await sender.Send(new GetProductsQuery(request));
                var response = result.Adapt<GetProductResponse>();
                return Results.Ok(response.Products);
            })
          .WithName("GetProduct")
          .Produces<GetProductResponse>(StatusCodes.Status201Created)
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .WithSummary("Product Details.")
          .WithDescription("Product Details.");
        }
    }
}
