using Shared.Exceptions;

namespace Ordering.Orders.Exceptions
{
    public class OrderNotFoundException(Guid Id)
        : NotFoundException("Order", Id)
    {
    }
}
