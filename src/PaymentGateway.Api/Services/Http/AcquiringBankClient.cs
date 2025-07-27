using FluentResults;
using PaymentGateway.Api.Services.Http.Models;

namespace PaymentGateway.Api.Services.Http
{
    public interface IAcquiringBankClient
    {
        Task<Result<AcquiringBankCreateProcessResponse>> CreatePaymentProcess(AcquiringBankCreateProcessRequest request);
    }

    public sealed class AcquiringBankClient : IAcquiringBankClient
    {
        private readonly HttpClient _httpClient;

        public AcquiringBankClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Result<AcquiringBankCreateProcessResponse>> CreatePaymentProcess(AcquiringBankCreateProcessRequest request)
        {
            //Should probably add some logs here in case of an error from the side of the acquiringBank
            //Some retry policy and circuit breaker would be nice aswell

            var response = await _httpClient.PostAsJsonAsync("payments", request);

            if (response.IsSuccessStatusCode)
                return Result.Ok(await response.Content.ReadFromJsonAsync<AcquiringBankCreateProcessResponse>());

            return Result.Fail(response.ReasonPhrase);
        }
    }
}
