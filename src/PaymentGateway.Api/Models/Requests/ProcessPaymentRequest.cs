using FluentValidation;

namespace PaymentGateway.Api.Models.Requests;

public sealed class ProcessPaymentRequest
{
    public string CardNumber { get; set; }
    public int ExpiryMonth { get; set; }
    public int ExpiryYear { get; set; }
    public string Currency { get; set; }
    public int Amount { get; set; }
    public int Cvv { get; set; }
}

public sealed class ProcessPaymentRequestValidator : AbstractValidator<ProcessPaymentRequest>
{
    public ProcessPaymentRequestValidator()
    {
        RuleFor(x => x.CardNumber)
            .NotEmpty()
            .Matches("^[0-9]+$").WithMessage("{PropertyName} must contain only numeric values")
            .Length(14, 19).WithMessage("{PropertyName} must be between 14-19 characters long");

        RuleFor(x => x.ExpiryMonth)
            .NotEmpty()
            .InclusiveBetween(1, 12).WithMessage("{PropertyName} must be a valid month");

        //RuleFor(x => x.CardNumber)
        //    .NotEmpty()
        //    .Matches("^[0-9]+$").WithMessage("{propertyName} must contain only numeric values")
        //    .Length(14, 19).WithMessage("{propertyName} must be between 14-19 characters long");

        //RuleFor(x => x.CardNumber)
        //    .NotEmpty()
        //    .Matches("^[0-9]+$").WithMessage("{propertyName} must contain only numeric values")
        //    .Length(14, 19).WithMessage("{propertyName} must be between 14-19 characters long");

        //RuleFor(x => x.CardNumber)
        //    .NotEmpty()
        //    .Matches("^[0-9]+$").WithMessage("{propertyName} must contain only numeric values")
        //    .Length(14, 19).WithMessage("{propertyName} must be between 14-19 characters long");

        //RuleFor(x => x.CardNumber)
        //    .NotEmpty()
        //    .Matches("^[0-9]+$").WithMessage("{propertyName} must contain only numeric values")
        //    .Length(14, 19).WithMessage("{propertyName} must be between 14-19 characters long");
    }
}