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
            var response = await _httpClient.PostAsJsonAsync("payments", request);

            if (response.IsSuccessStatusCode)
                return Result.Ok(await response.Content.ReadFromJsonAsync<AcquiringBankCreateProcessResponse>());

            return Result.Fail(response.ReasonPhrase);
        }
    }
}
