using FluentValidation;

namespace PaymentGateway.Api.Models.Requests;

public sealed class ProcessPaymentRequest
{
    public string CardNumber { get; set; }
    public int ExpiryMonth { get; set; }
    public int ExpiryYear { get; set; }
    public string Currency { get; set; }
    public int Amount { get; set; }
    public string Cvv { get; set; }
}

public sealed class ProcessPaymentRequestValidator : AbstractValidator<ProcessPaymentRequest>
{
    private static readonly string[] AllowedCurrencies = { "USD", "EUR", "BRL" };

    public ProcessPaymentRequestValidator()
    {
        RuleFor(x => x.CardNumber)
            .NotEmpty()
            .Matches("^[0-9]+$").WithMessage("{PropertyName} must contain only numeric values")
            .Length(14, 19).WithMessage("{PropertyName} must be between 14-19 characters long");

        RuleFor(x => x.ExpiryMonth)
            .NotEmpty()
            .InclusiveBetween(1, 12).WithMessage("{PropertyName} must be a valid month");

        RuleFor(x => x.ExpiryYear)
            .Must(expiryYear => expiryYear >= DateTime.UtcNow.Year).WithMessage("{PropertyName} must be greater or equal to the current year");

        RuleFor(x => x)
            .Must(processPaymentRequest => IsValidExpiryDate(processPaymentRequest.ExpiryYear, processPaymentRequest.ExpiryMonth))
            .WithMessage("Combination of ExpiryYear and ExpiryMonth must be in the future");

        //Not sure if i'm getting this validation logic correctly, but from what i understood, we have to make sure we validate against no more than 3 currency codes from the
        //https://www.xe.com/currency/ , and i'm guessing i can choose which ones.
        RuleFor(x => x.Currency)
            .NotEmpty()
            .Must(c => AllowedCurrencies.Contains(c)).WithMessage("{PropertyName} must be one of the following: USD, EUR, BRL.");

        RuleFor(x => x.Amount)
            .NotEmpty();

        RuleFor(x => x.Cvv)
            .NotEmpty()
            .Matches("^[0-9]+$").WithMessage("{PropertyName} must contain only numeric values")
            .Length(3, 4).WithMessage("{PropertyName} must be between 3-4 characters long");
    }

    private bool IsValidExpiryDate(int expiryYear, int expiryMonth)
    {
        if (expiryYear == DateTime.UtcNow.Year)
            return expiryMonth > DateTime.UtcNow.Month;

        return true;
    }
}