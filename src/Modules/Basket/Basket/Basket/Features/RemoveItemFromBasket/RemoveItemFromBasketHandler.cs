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
    internal class RemoveItemFromBasketHandler(BasketDbContext basketDb) : ICommandHandler<RemoveItemFromBasketCommand, RemoveItemFromBasketResult>
    {
        public async Task<RemoveItemFromBasketResult> Handle(RemoveItemFromBasketCommand request, CancellationToken cancellationToken)
        {
            var shoppingCart = await basketDb.ShoppingCarts
                .Include(c => c.Items)
                .SingleOrDefaultAsync(c => c.UserName == request.UserName, cancellationToken);
            if (shoppingCart == null)
            {
                throw new BasketNotFoundException(request.UserName);
            }
            shoppingCart.RemoveItem(request.ProductId);
            await basketDb.SaveChangesAsync(cancellationToken);
            return new RemoveItemFromBasketResult(shoppingCart.Id);
        }
    }
  
}
