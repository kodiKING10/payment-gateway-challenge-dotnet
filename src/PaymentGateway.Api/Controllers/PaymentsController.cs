using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Models.Responses;
using PaymentGateway.Api.Repositories;
using PaymentGateway.Api.Services;

namespace PaymentGateway.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentsController : Controller
{
    private readonly IPaymentsRepository _paymentsRepository;
    private readonly IPaymentsProcessorHandler _paymentsProcessorHandler;

    public PaymentsController(IPaymentsRepository paymentsRepository, IPaymentsProcessorHandler paymentsProcessorHandler)
    {
        _paymentsRepository = paymentsRepository;
        _paymentsProcessorHandler = paymentsProcessorHandler;
    }

    //Would add a exception handler middleware at least, probably not over-engineering to do so
    [HttpPost]
    public async Task<ActionResult<ProcessPaymentResponse?>> PostPayment(ProcessPaymentRequest request)
    {
        var result = await _paymentsProcessorHandler.Create(request);

        if (result.Errors.Any())
            return BadRequest(result.ValueOrDefault);

        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public ActionResult<ProcessPaymentResponse?> GetPayment(Guid id)
    {
        var payment = _paymentsRepository.Get(id);

        return payment is null ? NotFound() : Ok(payment);
    }
}