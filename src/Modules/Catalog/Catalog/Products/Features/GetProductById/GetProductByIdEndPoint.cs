namespace Catalog.Products.Features.GetProductById
{
    public record GetProductByIdResponse(ProductDto Product);
    public class GetProductByIdEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/{Id}", async (Guid Id, ISender sender) =>
            {
                var result = await sender.Send(new GetProductsByIdQuery(Id));
                var response = result.Adapt<GetProductByIdResponse>();
                return Results.Ok(response.Product);
            })
          .WithName("GetProductById")
          .Produces<GetProductByIdResponse>(StatusCodes.Status201Created)
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .WithSummary("Product Details.")
          .WithDescription("Product Details.");

        }
    }
}
