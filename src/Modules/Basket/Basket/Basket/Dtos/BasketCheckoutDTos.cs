namespace Basket.Basket.Dtos
{
    public record BasketCheckoutDTos
    (
        string UserName,
        Guid CustomerId,
        decimal TotalPrice,

        string FirstName,
        string LastName,
        string EmailAddress,
        string AddressLine,
        string State,
        string Country,
        string ZipCode,

        string CardName ,
        string CardNumber,
        string Expiration,
        string CVV,
        int PaymentMethod 
        );
}
