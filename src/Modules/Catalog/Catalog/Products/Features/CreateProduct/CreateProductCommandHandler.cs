using MediatR;

namespace Catalog.Products.Features.CreateProduct
{
    public record CreateProductCommand(
    string Name, List<string> Category, string Description, string ImageFile, decimal Price)
        : IRequest<CreateProudctResult>;
    public record CreateProudctResult(Guid ProductId);
    internal class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreateProudctResult>
    {
        public Task<CreateProudctResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
