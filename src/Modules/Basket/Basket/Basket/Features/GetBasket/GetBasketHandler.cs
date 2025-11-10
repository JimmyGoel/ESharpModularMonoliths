
namespace Basket.Basket.Features.GetBasket
{
    public record GetBasketQuery(string UserName)
        : IQuery<GetBasketResult>;
    public record GetBasketResult(ShoppingCartDto ShoppingCart);
    public class GetBasketHandler(BasketDbContext basketDb) : IQueryHandler<GetBasketQuery, GetBasketResult>
    {
        public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
        {

            var basket = await basketDb.ShoppingCarts
                .AsNoTracking()
                .Include(c => c.Items)
                .SingleOrDefaultAsync(
                    c => c.UserName == query.UserName,
                    cancellationToken);
            if (basket == null)
            {
                throw new BasketNotFoundException(query.UserName);
            }

            var basketDto = basket.Adapt<ShoppingCartDto>();

            return new GetBasketResult(basketDto);

        }
    }
}
