
namespace Basket.Basket.Features.UpdateItemPriceBasket
{
    public record UpdateItemPriceInBasketComand(Guid ProductId, decimal NewPrice)
        : ICommand<UpdateItemPriceInBasketResult>;
    public record UpdateItemPriceInBasketResult(bool IsPriceUpdated);

    public class UpdateItemPriceInBasketValidator
        : AbstractValidator<UpdateItemPriceInBasketComand>
    {
        public UpdateItemPriceInBasketValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("ProductId is required.");
            RuleFor(x => x.NewPrice)
                .GreaterThan(0).WithMessage("NewPrice must be greater than zero.");
        }
    }
    public class UpdateItemPriceInBasketHandler(BasketDbContext basketDb)
        : ICommandHandler<UpdateItemPriceInBasketComand, UpdateItemPriceInBasketResult>
    {
        public async Task<UpdateItemPriceInBasketResult> Handle(UpdateItemPriceInBasketComand request, CancellationToken cancellationToken)
        {
            var ItemToUpdate = await basketDb.ShoppingCartItems
                .Where(x => x.ProductId == request.ProductId).ToListAsync(cancellationToken);
            if (!ItemToUpdate.Any())
            {
                return new UpdateItemPriceInBasketResult(false);
            }

            foreach (var item in ItemToUpdate)
            {
                item.UpdatePrice(request.NewPrice);
            }
            await basketDb.SaveChangesAsync(cancellationToken);
            return new UpdateItemPriceInBasketResult(true);
        }
    }
}
