


namespace Catalog.Contracts.Products.Features.GetProductById
{
    public record GetProductsByIdQuery(Guid Id)
: IQuery<GetProductsByIdResult>;
    public record GetProductsByIdResult(ProductDto Product);
}
