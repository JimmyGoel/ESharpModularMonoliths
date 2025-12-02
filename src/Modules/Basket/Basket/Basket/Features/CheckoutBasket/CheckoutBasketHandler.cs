
using MassTransit;
using Shared.Messaging.Events;
using System.Text.Json;

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
    internal class CheckoutBasketHandler(BasketDbContext dbContext)
        : ICommandHandler<CheckoutBasketCommand, CheckoutBaksetResult>
    {
        public async Task<CheckoutBaksetResult> Handle(CheckoutBasketCommand request, CancellationToken cancellationToken)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var basket = await dbContext.ShoppingCarts
                .Include(c => c.Items)
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.UserName == request.BasketCheckout.UserName, cancellationToken)
                ?? throw new BasketNotFoundException(request.BasketCheckout.UserName);
                var eventMessage = request.BasketCheckout.Adapt<BasketCheckoutIntegrationEvents>();
                eventMessage.TotalPrice = basket.TotalPrice;

                // Using Outbox Pattern
                var outboxMessage = new OutboxMessage
                {
                    Id = Guid.NewGuid(),
                    OccuredOn = DateTime.UtcNow,
                    Type = typeof(BasketCheckoutIntegrationEvents).AssemblyQualifiedName!,
                    Data = JsonSerializer.Serialize(eventMessage)
                };

                dbContext.outboxMessages.Add(outboxMessage);

                //TODO : Remove basket after payment process
                //dbContext.ShoppingCarts.Remove(basket);

                await dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return new CheckoutBaksetResult(true);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                return new CheckoutBaksetResult(false);
            }

            /// WITHOUT OUTBOX PATTERN
            //var basket = await repository.GetBasket(request.BasketCheckout.UserName, true, cancellationToken);
            //var eventMessage = request.BasketCheckout.Adapt<BasketCheckoutIntegrationEvents>();
            //eventMessage.TotalPrice = basket!.TotalPrice;
            //await bus.Publish(eventMessage, cancellationToken);
            ////TODO : Remove basket after payment process
            ////await repository.DeleteBasket(request.BasketCheckout.UserName, cancellationToken);
            //return new CheckoutBaksetResult(true);
        }
    }
}
