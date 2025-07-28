using System.Text.Json.Serialization;

using FluentValidation;
using FluentValidation.AspNetCore;
using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Repositories;
using PaymentGateway.Api.Services;
using PaymentGateway.Api.Services.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<IValidator<ProcessPaymentRequest>, ProcessPaymentRequestValidator>();

builder.Services.AddScoped<IPaymentsProcessorHandler, PaymentsProcessorHandler>();
builder.Services.AddSingleton<IPaymentsRepository, PaymentsRepository>();
builder.Services.AddHttpClient<IAcquiringBankClient, AcquiringBankClient>(x =>
    x.BaseAddress = new Uri("http://localhost:8080/"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
