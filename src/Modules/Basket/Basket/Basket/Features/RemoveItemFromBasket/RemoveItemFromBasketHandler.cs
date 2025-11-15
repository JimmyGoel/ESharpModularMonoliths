namespace Basket.Basket.Features.RemoveItemFromBasket
{
    public record RemoveItemFromBasketCommand(string UserName, Guid ProductId)
        : ICommand<RemoveItemFromBasketResult>;
    public record RemoveItemFromBasketResult(Guid Id);

    public class RemoveItemFromBasketValidation : AbstractValidator<RemoveItemFromBasketCommand>
    {
        public RemoveItemFromBasketValidation()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName Required");
            RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId Required");
        }
    }
    public class RemoveItemFromBasketHandler(IBasketRepository basketRepository) : ICommandHandler<RemoveItemFromBasketCommand, RemoveItemFromBasketResult>
    {
        public async Task<RemoveItemFromBasketResult> Handle(RemoveItemFromBasketCommand request, CancellationToken cancellationToken)
        {
            var shoppingCart = await basketRepository.GetBasket(request.UserName, false, cancellationToken: cancellationToken);
            shoppingCart?.RemoveItem(request.ProductId);
            await basketRepository.SaveChangesAsync(cancellationToken);
            return new RemoveItemFromBasketResult(shoppingCart!.Id);
        }
    }

}
