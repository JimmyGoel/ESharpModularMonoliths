﻿

namespace Catalog.Products.Features.CreateProduct
{
    public record CreateProductCommand(ProductDto Product)
        : ICommand<CreateProudctResult>;
    public record CreateProudctResult(Guid ProductId);
    internal class CreateProductHandler(CatalogDbContext dbContext) : ICommandHandler<CreateProductCommand, CreateProudctResult>
    {
        public async Task<CreateProudctResult> Handle(CreateProductCommand command,
            CancellationToken cancellationToken)
        {
            var product = CreateNewProduct(command.Product);
            await dbContext.SaveChangesAsync(cancellationToken);
            return new CreateProudctResult(product.Id);

        }

        private Product CreateNewProduct(ProductDto productDTO)
        {
            var product = Product.Create(
                productDTO.Name,
                productDTO.Category,
                productDTO.Description,
                productDTO.ImageFile,
                productDTO.Price);
            return product;
        }
    }
}
