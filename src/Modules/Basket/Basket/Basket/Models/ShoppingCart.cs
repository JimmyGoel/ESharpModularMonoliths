

namespace Basket.Basket.Models
{
    public class ShoppingCart : Aggregrate<Guid>
    {
        public string UserName { get; private set; } = default!;
        private readonly List<ShoppingCartItem> _items = new();

        public IReadOnlyList<ShoppingCartItem> Items
            => _items.AsReadOnly();
        public decimal TotalPrice
            => Items.Sum(item => item.Price * item.Quantity);

        public static ShoppingCart CreateNew(string userName)
        {
            ArgumentException.ThrowIfNullOrEmpty(userName, nameof(userName));
            var cart = new ShoppingCart
            {
                Id = Guid.NewGuid(),
                UserName = userName
            };
            return cart;
        }
        public void AddItem(ShoppingCartItem item)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(item.Quantity, nameof(item.Quantity));
            ArgumentOutOfRangeException.ThrowIfNegative(item.Price, nameof(item.Price));

            var existingItem = _items.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                _items.Add(item);
            }
        }
        public void RemoveItem(Guid productId)
        {
            var item = _items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                _items.Remove(item);
            }
        }
    }
}
