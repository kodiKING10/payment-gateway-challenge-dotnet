namespace PaymentGateway.Api.Models;

public enum PaymentStatus
{
    Authorized,
    Declined,
    Rejected
    // I dont really get the necessity of this status, since it's a validation error in the PaymentGateway and in the specification we only expect to return Authorized or Declined
}