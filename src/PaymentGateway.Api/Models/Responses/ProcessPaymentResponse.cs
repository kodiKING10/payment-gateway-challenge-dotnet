using PaymentGateway.Api.Models.Requests;

namespace PaymentGateway.Api.Models.Responses;

public sealed class ProcessPaymentResponse
{
    public ProcessPaymentResponse()
    {

    }

    public ProcessPaymentResponse(ProcessPaymentRequest request, PaymentStatus status)
    {
        Id = Guid.NewGuid();
        Amount = request.Amount;
        Currency = request.Currency;
        ExpiryMonth = request.ExpiryMonth;
        ExpiryYear = request.ExpiryYear;
        Status = status;
        LastFourCardDigits = int.Parse(request.CardNumber.Substring(request.CardNumber.Length - 4));
    }

    public Guid Id { get; set; }
    public PaymentStatus Status { get; set; }
    public int LastFourCardDigits { get; set; }
    public int ExpiryMonth { get; set; }
    public int ExpiryYear { get; set; }
    public string Currency { get; set; }
    public int Amount { get; set; }
}
