
namespace Catalog.Products.Features.DeleteProduct
{
    public record DeleteProductRequest(string Id);
    public record DeleteProductResponse(bool IsSuccess);
    public class DeleteProductEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/products", async (DeleteProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<DeleteProductCommand>();
                var result = await sender.Send(command);
                var response = request.Adapt<DeleteProductResponse>();
                return Results.Ok(response);
            })
          .WithName("DeleteProduct")
          .Produces<DeleteProductResponse>(StatusCodes.Status201Created)
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .WithSummary("Product Delete Successfully.")
          .WithDescription("Product Delete Successfully.");

        }
    }
}
