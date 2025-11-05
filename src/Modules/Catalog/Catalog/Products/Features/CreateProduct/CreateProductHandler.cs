

namespace Catalog.Products.Features.CreateProduct
{
    public record CreateProductCommand(ProductDto Product)
        : ICommand<CreateProudctResult>;
    public record CreateProudctResult(Guid Id);

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(c => c.Product).NotNull();
            RuleFor(c => c.Product.Name).NotEmpty().MaximumLength(200).WithMessage("Name is required");
            RuleFor(c => c.Product.Category).NotEmpty().WithMessage("Category is required");
            RuleFor(c => c.Product.Description).NotEmpty().WithMessage("Desc is required");
            RuleFor(c => c.Product.ImageFile).NotEmpty().WithMessage("ImageFile is required");
            RuleFor(c => c.Product.Price).GreaterThan(0).WithMessage("Price Greater than zero");
        }
    }
    public class CreateProductHandler(CatalogDbContext dbContext) : ICommandHandler<CreateProductCommand, CreateProudctResult>
    {
        public async Task<CreateProudctResult> Handle(CreateProductCommand command,
            CancellationToken cancellationToken)
        {
            var product = CreateNewProduct(command.Product);
            dbContext.Products.Add(product);
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
