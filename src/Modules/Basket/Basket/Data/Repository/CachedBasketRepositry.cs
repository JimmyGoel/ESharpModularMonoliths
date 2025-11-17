
using Basket.Data.JsonConvertor;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Basket.Data.Repository
{
    public class CachedBasketRepositry(
        IBasketRepository repository,
        IDistributedCache cache) : IBasketRepository
    {
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new ShoppingCartConvertor(), new ShoppingCartItemConvertor() }
        };
        public async Task<ShoppingCart?> GetBasket(string userName, bool asNoTracking = true, CancellationToken cancellationToken = default)
        {
            if (!asNoTracking)
            {
                //bypass cache for tracking queries
                return await repository.GetBasket(userName, asNoTracking, cancellationToken);
            }
            var cacheKey = $"basket_{userName}";
            var cachedBasket = await cache.GetStringAsync(cacheKey, cancellationToken);
            if (!string.IsNullOrEmpty(cachedBasket))
            {
                return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket, _options);
            }
            var basket = await repository.GetBasket(userName, asNoTracking, cancellationToken);
            if (basket != null)
            {
                var serializedBasket = JsonSerializer.Serialize(basket, _options);
                await cache.SetStringAsync(cacheKey, serializedBasket, cancellationToken);
            }
            return basket;
        }



        public async Task<ShoppingCart?> CreateBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
        {
            await repository.CreateBasket(basket, cancellationToken);
            var cacheKey = $"basket_{basket.UserName}";
            var serializedBasket = JsonSerializer.Serialize(basket, _options);
            await cache.SetStringAsync(cacheKey, serializedBasket, cancellationToken);
            return basket;
        }

        public async Task<bool?> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            await repository.DeleteBasket(userName, cancellationToken);
            var cacheKey = $"basket_{userName}";
            await cache.RemoveAsync(cacheKey, cancellationToken);
            return true;
        }


        public async Task<int> SaveChangesAsync(string? userName = null, CancellationToken cancellationToken = default)
        {
            var basekt = await repository.SaveChangesAsync(userName, cancellationToken);
            if (!string.IsNullOrEmpty(userName))
            {
                var cacheKey = $"basket_{userName}";
                await cache.RemoveAsync(cacheKey, cancellationToken);
            }
            return basekt;

        }
    }
}
