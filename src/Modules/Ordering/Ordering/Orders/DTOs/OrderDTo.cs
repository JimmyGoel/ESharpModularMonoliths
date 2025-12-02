namespace Ordering.Orders.DTOs
{
    public record OrderDTo
    (
        Guid Id,
        Guid CustomerId,
        string OrderName,
        AddressDTo ShippingAddress,
        AddressDTo BillingAddress,
        PaymentDto Payment,
        List<OrderItemDto> OrderItems
    );
}
