
namespace Basket.Data.Repository
{
    public class BasketRepository(BasketDbContext basketDb) : IBasketRepository
    {
        public async Task<ShoppingCart?> CreateBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
        {
            basketDb.ShoppingCarts.Add(basket);
            await basketDb.SaveChangesAsync(cancellationToken);
            return basket;
        }

        public async Task<bool?> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            var basket = await GetBasket(userName, false, cancellationToken);
            basketDb.ShoppingCarts.Remove(basket!);
            await basketDb.SaveChangesAsync();
            return true;
        }

        public async Task<ShoppingCart?> GetBasket(string userName, bool asNoTracking = true, CancellationToken cancellationToken = default)
        {
            var query = basketDb.ShoppingCarts
                .Include(c => c.Items)
                .Where(c => c.UserName == userName);
            if (asNoTracking) query = query.AsNoTracking();
            var result = await query.SingleOrDefaultAsync(cancellationToken);

            return result ?? throw new BasketNotFoundException(userName);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await basketDb.SaveChangesAsync(cancellationToken);
        }
    }
}
