
namespace Catalog.Products.Features.GetProducts
{
    public record GetProductsQuery(PaginationRequest request)
   : IQuery<GetProductsResult>;
    public record GetProductsResult(PaginatedResult<ProductDto> Products);
    internal class GetProductsHandler(CatalogDbContext dbContext)
            : IQueryHandler<GetProductsQuery, GetProductsResult>
    {
        public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            var pageIndex = query.request.PageIndex;
            var pageSize = query.request.PageSize;

            var totalCount = await dbContext.Products.LongCountAsync(cancellationToken);


            var products = await dbContext.Products
                .AsNoTracking()
                .OrderBy(e => e.Name)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var productsDto = products.Adapt<List<ProductDto>>();

            return new GetProductsResult(
                new PaginatedResult<ProductDto>(
                    pageIndex,
                    pageSize,
                    totalCount,
                    productsDto)
                );
        }
    }
}
