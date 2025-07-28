using PaymentGateway.Api.Models.Responses;

namespace PaymentGateway.Api.Repositories;

public interface IPaymentsRepository
{
    void Add(ProcessPaymentResponse payment);

    ProcessPaymentResponse Get(Guid id);
}

public sealed class PaymentsRepository : IPaymentsRepository
{
    //I decided to create another folder Repositories to easily find the classes related to data persistence
    //With a real database should probably make the identifier unique to avoid duplicates, with this implementation duplicates are possible
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