﻿

namespace Catalog.Products.Features.GetProductById
{
    public record GetProductsByIdQuery(Guid Id)
   : IQuery<GetProductsByIdResult>;
    public record GetProductsByIdResult(ProductDto Product);
    internal class GetProductByIdHandler(CatalogDbContext dbContext)
            : IQueryHandler<GetProductsByIdQuery, GetProductsByIdResult>
    {
        public async Task<GetProductsByIdResult> Handle(GetProductsByIdQuery query, CancellationToken cancellationToken)
        {
            var product = await dbContext
                .Products
                .AsNoTracking()
                .SingleOrDefaultAsync(e => e.Id == query.Id, cancellationToken);
            if (product is null)
            {
                throw new Exception($"Product Not found : {query.Id}");
            }
            var productDto = product.Adapt<ProductDto>();
            return new GetProductsByIdResult(productDto);
        }
    }
    
}
