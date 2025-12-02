
namespace Ordering.Orders.Models
{
    public class Order : Aggregrate<Guid>
    {
        private readonly List<OrderItem> _orderItems = new();

        public IReadOnlyList<OrderItem> OrderItems
            => _orderItems.AsReadOnly();

        public Guid CustomerId { get; private set; } = default!;
        public string OrderName { get; private set; } = default!;
        public Address ShippingAddress { get; private set; } = default!;
        public Address BillingAddress { get; private set; } = default!;
        public Payment Payment { get; private set; } = default!;

        public decimal TotalPrice => OrderItems.Sum(x => x.Price * x.Quantity);

        public static Order Create(Guid id, Guid customerId, string orderName,
                                   Address shippingAddress, Address billingAddress,
                                   Payment payment)
        {
            var order = new Order
            {
                Id = id,
                CustomerId = customerId,
                OrderName = orderName,
                ShippingAddress = shippingAddress,
                BillingAddress = billingAddress,
                Payment = payment
            };
            order.AddDomainEvent(new OrderCreateEvent(order));
            return order;
        }
        public void Add(Guid productId, int quantity, decimal price)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity, nameof(quantity));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price, nameof(price));

            var itemExists = _orderItems.FirstOrDefault(x => x.ProductId == productId);

            if (itemExists is not null)
            {
                itemExists.Quantity += quantity;
            }
            else
            {
                var orderItem = new OrderItem(Id, productId, quantity, price);
                _orderItems.Add(orderItem);
            }
        }
        public void Remove(Guid productId)
        {

            var item = _orderItems.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                _orderItems.Remove(item);
            }

        }
    }
}
