using Shared.Exceptions;

namespace Basket.Basket.Exceptions
{
    public class BasketNotFoundException(string UserName) :
        NotFoundException("ShoppingCart", UserName)
    {
    }
}
