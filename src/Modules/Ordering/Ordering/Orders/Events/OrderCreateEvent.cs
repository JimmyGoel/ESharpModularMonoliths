using Ordering.Orders.Models;

namespace Ordering.Orders.Events
{
    public record OrderCreateEvent(Order order)
        : IDomainEvent;
}
