using Catalog.Products.Features.GetProductById;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Catalog.Products.Features.GetProductByCategory
{
    public record GetProductsByCategoryQuery(string category)
  : IQuery<GetProductsByCategoryResult>;
    public record GetProductsByCategoryResult(IEnumerable<ProductDto> Products);
    internal class GetProductByCategoryhandler(CatalogDbContext dbContext)
        : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
    {
        public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
        {
            var product = await dbContext
                .Products
                .AsNoTracking()
                .Where(e => e.Category.Contains(request.category))
                .ToListAsync(cancellationToken);
            if (product is null)
            {
                throw new Exception($"Category Not found : {request.category}");
            }
            var productDto = product.Adapt<IEnumerable<ProductDto>>();
            return new GetProductsByCategoryResult(productDto);
        }
    }
}
