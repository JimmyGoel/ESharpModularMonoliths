

namespace Basket.Basket.Features.CreateBasket
{
    public record CreateBasketCommand(ShoppingCartDto ShoppingCart) : ICommand<CreateBasketResult>;
    public record CreateBasketResult(Guid Id);

    public class CreateBasketValidator : AbstractValidator<CreateBasketCommand>
    {
        public CreateBasketValidator()
        {
            RuleFor(x => x.ShoppingCart.UserName).NotEmpty().WithMessage("UserName is required");
        }
    }
    internal class CreateBasketHandler(IBasketRepository basketRepository)
        : ICommandHandler<CreateBasketCommand, CreateBasketResult>
    {
        public async Task<CreateBasketResult> Handle(CreateBasketCommand request, CancellationToken cancellationToken)
        {
            var shoppingCart = CreateNewBasket(request.ShoppingCart);

            await basketRepository.CreateBasket(shoppingCart, cancellationToken);

            return new CreateBasketResult(shoppingCart.Id);
        }

        private ShoppingCart CreateNewBasket(ShoppingCartDto shoppingCartDto)
        {
            var shoppingCart = ShoppingCart.CreateNew(shoppingCartDto.UserName);

            shoppingCartDto.Items.ForEach(itemDto =>
            {
                var item = new ShoppingCartItem
                (
                    itemDto.Id,
                    itemDto.ProductId,
                    itemDto.Quantity,
                    itemDto.ProductName,
                    itemDto.Price,
                    itemDto.Color
                );
                shoppingCart.AddItem(item);
            });

            return shoppingCart;
        }
    }
}
