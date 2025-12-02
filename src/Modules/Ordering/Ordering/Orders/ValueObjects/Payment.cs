using System.Net.Mail;

namespace Ordering.Orders.ValueObjects
{
    public record Payment
    {
        public string? CardName { get; } = default!;
        public string? CardNumber { get; } = default!;
        public string? Expiration { get; } = default!;
        public string? CVV { get; } = default!;
        public int PaymentMethod { get; } = default!;
        protected Payment()
        {
        }
        private Payment(string cardName, string cardNumber, string expiration, string cVV, int paymentMethord)
        {
            CardName = cardName;
            CardNumber = cardNumber;
            Expiration = expiration;
            CVV = cVV;
            PaymentMethod = paymentMethord;
        }
        public static Payment Of(string? cardName, string? cardNumber, string? expiration, string? cvv, int paymentMethod)
        {
            ArgumentException.ThrowIfNullOrEmpty(cardName, nameof(cardName));
            ArgumentException.ThrowIfNullOrEmpty(cardNumber, nameof(cardNumber));
            ArgumentException.ThrowIfNullOrEmpty(cvv, nameof(cvv));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(cvv.Length, 3);

            return new Payment(cardName!, cardNumber!, expiration!, cvv!, paymentMethod);

        }
    }
}
