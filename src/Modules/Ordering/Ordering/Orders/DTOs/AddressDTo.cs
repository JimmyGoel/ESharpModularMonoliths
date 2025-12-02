namespace Ordering.Orders.DTOs
{
    public record AddressDTo(string FirstName, string LastName
        ,string EmailAddress,string AddressLine, string State, string Country, string ZipCode);
}
