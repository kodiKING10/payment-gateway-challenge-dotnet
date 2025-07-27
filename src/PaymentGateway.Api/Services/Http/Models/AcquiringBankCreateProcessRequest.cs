using System.Text.Json.Serialization;

namespace PaymentGateway.Api.Services.Http.Models
{
    public sealed class AcquiringBankCreateProcessRequest
    {
        public AcquiringBankCreateProcessRequest(string cardNumber, string expiryDate, string currency, int amount, string cvv)
        {
            CardNumber = cardNumber;
            ExpiryDate = expiryDate;
            Currency = currency;
            Amount = amount;
            CVV = cvv;
        }

        [JsonPropertyName("card_number")]
        public string CardNumber { get; set; }
        [JsonPropertyName("expiry_date")]
        public string ExpiryDate { get; set; }
        public string Currency { get; set; }
        public int Amount { get; set; }
        public string CVV { get; set; }
    }
}
