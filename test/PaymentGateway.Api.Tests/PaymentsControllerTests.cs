using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using PaymentGateway.Api.Controllers;
using PaymentGateway.Api.Models.Responses;
using PaymentGateway.Api.Repositories;
using PaymentGateway.Api.Services;
using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Models;

namespace PaymentGateway.Api.Tests;

public class PaymentsControllerTests
{
    private readonly Random _random = new();

    //Could make a theory here with the tests cases below to remove duplicate code
    [Fact]
    public async Task Given_card_number_with_zero_as_the_final_number_should_return_bad_request()
    {
        // Arrange
        var payment = new ProcessPaymentRequest
        {
            ExpiryYear = _random.Next(2030, 2037),
            ExpiryMonth = _random.Next(1, 12),
            Amount = _random.Next(1000, 100000),
            CardNumber = "123905840384890",
            Cvv = "456",
            Currency = "BRL"
        };

        var webApplicationFactory = new WebApplicationFactory<Program>();
        var client = webApplicationFactory.WithWebHostBuilder(builder =>
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<IPaymentsRepository>();
                services.AddSingleton<IPaymentsRepository>(new PaymentsRepository());
            }))
            .CreateClient();

        // Act
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };

        var response = await client.PostAsJsonAsync($"/api/Payments", payment);
        var paymentResponse = await response.Content.ReadFromJsonAsync<ProcessPaymentResponse?>(options);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(paymentResponse);
        Assert.Equal(PaymentStatus.Declined, paymentResponse.Status);
    }

    [Fact]
    public async Task Given_card_number_that_ends_with_even_number_as_the_final_number_should_return_ok_with_status_declined()
    {
        // Arrange
        var payment = new ProcessPaymentRequest
        {
            ExpiryYear = _random.Next(2030, 2037),
            ExpiryMonth = _random.Next(1, 12),
            Amount = _random.Next(1000, 100000),
            CardNumber = "123905840384892",
            Cvv = "456",
            Currency = "BRL"
        };

        var webApplicationFactory = new WebApplicationFactory<Program>();
        var client = webApplicationFactory.WithWebHostBuilder(builder =>
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<IPaymentsRepository>();
                services.AddSingleton<IPaymentsRepository>(new PaymentsRepository());
            }))
            .CreateClient();

        // Act
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };

        var response = await client.PostAsJsonAsync($"/api/Payments", payment);
        var paymentResponse = await response.Content.ReadFromJsonAsync<ProcessPaymentResponse?>(options);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(paymentResponse);
        Assert.Equal(PaymentStatus.Declined, paymentResponse.Status);
    }

    [Fact]
    public async Task Given_card_number_that_ends_with_odd_number_as_the_final_number_should_return_ok_with_status_authorized()
    {
        // Arrange
        var payment = new ProcessPaymentRequest
        {
            ExpiryYear = _random.Next(2030, 2037),
            ExpiryMonth = _random.Next(1, 12),
            Amount = _random.Next(1000, 100000),
            CardNumber = "123905840384893",
            Cvv = "456",
            Currency = "BRL"
        };

        var webApplicationFactory = new WebApplicationFactory<Program>();
        var client = webApplicationFactory.WithWebHostBuilder(builder =>
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<IPaymentsRepository>();
                services.AddSingleton<IPaymentsRepository>(new PaymentsRepository());
            }))
            .CreateClient();

        // Act
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };

        var response = await client.PostAsJsonAsync($"/api/Payments", payment);
        var paymentResponse = await response.Content.ReadFromJsonAsync<ProcessPaymentResponse?>(options);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(paymentResponse);
        Assert.Equal(PaymentStatus.Authorized, paymentResponse.Status);
    }

    [Fact]
    public async Task RetrievesAPaymentSuccessfully()
    {
        // Arrange

        //Could user Faker or Fixture to facilitate object creation
        var payment = new ProcessPaymentResponse()
        {
            Id = Guid.NewGuid(),
            ExpiryYear = _random.Next(2023, 2030),
            ExpiryMonth = _random.Next(1, 12),
            Amount = _random.Next(1, 10000),
            LastFourCardDigits = _random.Next(1111, 9999),
            Currency = "GBP"
        };

        var paymentsRepository = new PaymentsRepository();
        paymentsRepository.Add(payment);

        var webApplicationFactory = new WebApplicationFactory<Program>();
        var client = webApplicationFactory.WithWebHostBuilder(builder =>
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<IPaymentsRepository>();
                services.AddSingleton<IPaymentsRepository>(paymentsRepository);
            }))
            .CreateClient();

        // Act
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };

        var response = await client.GetAsync($"/api/Payments/{payment.Id}");
        var paymentResponse = await response.Content.ReadFromJsonAsync<ProcessPaymentResponse?>(options);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(paymentResponse);
    }

    [Fact]
    public async Task Returns404IfPaymentNotFound()
    {
        // Arrange
        var webApplicationFactory = new WebApplicationFactory<PaymentsController>();
        var client = webApplicationFactory.CreateClient();

        // Act
        var response = await client.GetAsync($"/api/Payments/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}