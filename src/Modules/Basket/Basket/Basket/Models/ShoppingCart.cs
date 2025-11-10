

namespace Basket.Basket.Models
{
    public class ShoppingCart : Aggregrate<Guid>
    {
        public string UserName { get; private set; } = default!;
        public List<ShoppingCartItem> Items { get; private set; } = new();

        public IReadOnlyList<ShoppingCartItem> items()
            => Items.AsReadOnly();
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

            var existingItem = Items.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                Items.Add(item);
            }
        }
        public void RemoveItem(Guid productId)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                Items.Remove(item);
            }
        }
    }
}
