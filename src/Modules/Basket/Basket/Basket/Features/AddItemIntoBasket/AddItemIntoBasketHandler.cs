
namespace Basket.Basket.Features.AddItemIntoBasket
{
    public record AddItemIntoBasketCommand(string UserName, ShoppingCartItemDto Item)
        : ICommand<AddItemIntoBasketResult>;
    public record AddItemIntoBasketResult(Guid Id);

    public class AddItemIntoBasketValidation : AbstractValidator<AddItemIntoBasketCommand>
    {
        public AddItemIntoBasketValidation()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName Required");
            RuleFor(x => x.Item.ProductId).NotEmpty().WithMessage("ProductId Required");
            RuleFor(x => x.Item.Quantity).GreaterThan(0).WithMessage("Quantity must be greater then 0");
        }
    }
    public class AddItemIntoBasketHandler(IBasketRepository basketRepository) : ICommandHandler<AddItemIntoBasketCommand, AddItemIntoBasketResult>
    {
        public async Task<AddItemIntoBasketResult> Handle(AddItemIntoBasketCommand request, CancellationToken cancellationToken)
        {
            var shoppingCart = await basketRepository.GetBasket(request.UserName, false, cancellationToken: cancellationToken);
            var item = new ShoppingCartItem
               (
                   request.Item.Id,
                   request.Item.ProductId,
                   request.Item.Quantity,
                   request.Item.ProductName,
                   request.Item.Price,
                   request.Item.Color
               );

            shoppingCart?.AddItem(item);
            await basketRepository.SaveChangesAsync(request.UserName, cancellationToken);
            return new AddItemIntoBasketResult(shoppingCart!.Id);
        }
    }
}
