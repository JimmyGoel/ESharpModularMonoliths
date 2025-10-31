namespace Catalog.Products.Features.DeleteProduct
{
    public record DeleteProductCommand(ProductDto Product)
   : ICommand<DeleteProductResult>;
    public record DeleteProductResult(bool IsSuccess);
    internal class DeleteProductHandler(CatalogDbContext dbContext)
        : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            var product = await dbContext.Products
               .FindAsync([command.Product.Id], cancellationToken);
            if (product is null)
            {
                throw new Exception($"Product Not found : {command.Product.Id}");
            }
            dbContext.Products.Remove(product);
            await dbContext.SaveChangesAsync(cancellationToken);
            return new DeleteProductResult(true);
        }
    }
}
