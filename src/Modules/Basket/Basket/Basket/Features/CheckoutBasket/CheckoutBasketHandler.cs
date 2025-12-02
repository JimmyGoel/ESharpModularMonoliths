
using MassTransit;
using Shared.Messaging.Events;

namespace Basket.Basket.Features.CheckoutBasket
{
    public record CheckoutBasketCommand(BasketCheckoutDTos BasketCheckout)
        : ICommand<CheckoutBaksetResult>;
    public record CheckoutBaksetResult(bool IsSuccess);
    public class CheckoutBasketValidator : AbstractValidator<CheckoutBasketCommand>
    {
        public CheckoutBasketValidator()
        {
            RuleFor(x => x.BasketCheckout).NotNull().WithMessage("BasketCheckout is required");
            RuleFor(x => x.BasketCheckout.UserName).NotEmpty().WithMessage("UserName is required");
        }
    }
    internal class CheckoutBasketHandler(IBasketRepository repository, IBus bus)
        : ICommandHandler<CheckoutBasketCommand, CheckoutBaksetResult>
    {
        public async Task<CheckoutBaksetResult> Handle(CheckoutBasketCommand request, CancellationToken cancellationToken)
        {
            var basket = await repository.GetBasket(request.BasketCheckout.UserName, true, cancellationToken);
            var eventMessage = request.BasketCheckout.Adapt<BasketCheckoutIntegrationEvents>();
            eventMessage.TotalPrice = basket!.TotalPrice;
            await bus.Publish(eventMessage, cancellationToken);
            //TODO : Remove basket after payment process
            //await repository.DeleteBasket(request.BasketCheckout.UserName, cancellationToken);
            return new CheckoutBaksetResult(true);
        }
    }
}
