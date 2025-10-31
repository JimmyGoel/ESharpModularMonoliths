
namespace Catalog.Products.Features.GetProducts
{
    public record GetProductsQuery()
   : IQuery<GetProductsResult>;
    public record GetProductsResult(IEnumerable<ProductDto> Products);
    internal class GetProductsHandler(CatalogDbContext dbContext)
            : IQueryHandler<GetProductsQuery, GetProductsResult>
    {
        public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            var products = await dbContext
                .Products
                .AsNoTracking()
                .OrderBy(e => e.Name)
                .ToListAsync(cancellationToken);

            var productsDto = products.Adapt<IEnumerable<ProductDto>>();

            return new GetProductsResult(productsDto);
        }
    }
}
