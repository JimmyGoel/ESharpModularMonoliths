
namespace Basket.Basket.Features.DeleteBasket
{
    public record DeleteBasketCommand(string UserName)
        : ICommand<DeleteBasketResult>;
    public record DeleteBasketResult(bool Success);
    public class DeleteBasketHandler(BasketDbContext basketDb)
        : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
    {
        public async Task<DeleteBasketResult> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
        {
            var shoppingCart = await basketDb.ShoppingCarts
                .SingleOrDefaultAsync(c => c.UserName == request.UserName, cancellationToken);
            if (shoppingCart == null)
            {
                throw new BasketNotFoundException(request.UserName);
            }
            basketDb.ShoppingCarts.Remove(shoppingCart);
            await basketDb.SaveChangesAsync(cancellationToken);
            return new DeleteBasketResult(true);
        }

    }
}
