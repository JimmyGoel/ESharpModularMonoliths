namespace Basket.Basket.Dtos
{
    public record ShoppingCartItemDto
    (
        Guid Id,
        Guid ShoppingCartId,
        Guid ProductId,
        string ProductName,
        decimal Price,
        int Quantity,
        string Color
    );
}
