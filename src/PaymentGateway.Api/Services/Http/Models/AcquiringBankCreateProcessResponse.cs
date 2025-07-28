using System.Text.Json.Serialization;

namespace PaymentGateway.Api.Services.Http.Models
{
    public sealed class AcquiringBankCreateProcessResponse
    {
        public bool Authorized { get; set; }

        [JsonPropertyName("authorization_code")]
        public string AuthorizationCode { get; set; }
    }
}
