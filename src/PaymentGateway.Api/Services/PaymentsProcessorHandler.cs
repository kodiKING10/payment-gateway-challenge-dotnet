using FluentResults;
using PaymentGateway.Api.Models;
using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Models.Responses;
using PaymentGateway.Api.Repositories;
using PaymentGateway.Api.Services.Http;
using PaymentGateway.Api.Services.Http.Models;

namespace PaymentGateway.Api.Services
{
    public interface IPaymentsProcessorHandler
    {
        Task<Result<ProcessPaymentResponse>> Create(ProcessPaymentRequest request);
    }

    public sealed class PaymentsProcessorHandler : IPaymentsProcessorHandler
    {
        private readonly IAcquiringBankClient _acquiringBankClient;
        private readonly IPaymentsRepository _paymentsRepository;

        public PaymentsProcessorHandler(IAcquiringBankClient acquiringBankClient, IPaymentsRepository paymentsRepository)
        {
            _acquiringBankClient = acquiringBankClient;
            _paymentsRepository = paymentsRepository;
        }

        public async Task<Result<ProcessPaymentResponse>> Create(ProcessPaymentRequest request)
        {
            //Would add unit tests to this class and constructors if the integration tests didn`t covered almost everything
            var acquiringBankRequest
                = new AcquiringBankCreateProcessRequest(request.CardNumber, $"{request.ExpiryMonth}/{request.ExpiryYear}", request.Currency, request.Amount, request.Cvv);

            var processResult = await _acquiringBankClient.CreatePaymentProcess(acquiringBankRequest);

            if (processResult.IsFailed)
                return Result.Ok(SaveAcquiringBankResponse(request, PaymentStatus.Declined)).WithErrors(processResult.Errors);

            var paymentStatus = processResult.Value.Authorized ? PaymentStatus.Authorized : PaymentStatus.Declined;

            return Result.Ok(SaveAcquiringBankResponse(request, paymentStatus));
        }

        private ProcessPaymentResponse SaveAcquiringBankResponse(ProcessPaymentRequest request, PaymentStatus status)
        {
            var processPaymentResponse = new ProcessPaymentResponse(request, status);

            _paymentsRepository.Add(processPaymentResponse);

            return processPaymentResponse;
        }
    }
}
