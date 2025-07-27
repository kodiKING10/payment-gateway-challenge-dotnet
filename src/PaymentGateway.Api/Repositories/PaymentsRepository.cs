using PaymentGateway.Api.Models.Responses;

namespace PaymentGateway.Api.Repositories;

public interface IPaymentsRepository
{
    void Add(ProcessPaymentResponse payment);

    ProcessPaymentResponse Get(Guid id);
}

public sealed class PaymentsRepository : IPaymentsRepository
{
    //With a real database should probably make the identifier unique to avoid duplicates
    public List<ProcessPaymentResponse> Payments = new();

    public void Add(ProcessPaymentResponse payment)
    {
        Payments.Add(payment);
    }

    public ProcessPaymentResponse Get(Guid id)
    {
        return Payments.FirstOrDefault(p => p.Id == id)!;
    }
}