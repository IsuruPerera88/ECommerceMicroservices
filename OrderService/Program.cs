using Microsoft.EntityFrameworkCore;
using OrderService.Clients;
using OrderService.Clients.Interfaces;
using OrderService.Data;
using OrderService.Extensions;
using OrderService.Repositories;
using OrderService.Repositories.Interfaces;
using OrderService.Services;
using OrderService.Services.Interfaces;
using Polly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("OrderDatabase")));

builder.Services
    .AddHttpClient<IProductClient, ProductClient>(
        client =>
        {
            client.BaseAddress = new Uri(
                builder.Configuration[
                    "Services:ProductService"]!);

            client.Timeout =
                TimeSpan.FromSeconds(5);
        })
    .AddStandardResilienceHandler(options =>
    {
        options.Retry.MaxRetryAttempts = 3;

        options.Retry.Delay =
            TimeSpan.FromSeconds(1);

        options.Retry.BackoffType =
            DelayBackoffType.Exponential;

        options.CircuitBreaker.SamplingDuration =
            TimeSpan.FromSeconds(30);

        options.CircuitBreaker.FailureRatio =
            0.5;

        options.CircuitBreaker.MinimumThroughput =
            5;

        options.CircuitBreaker.BreakDuration =
            TimeSpan.FromSeconds(30);
    });

builder.Services.AddAutoMapper(
    typeof(Program).Assembly);

builder.Services.AddScoped<
    IOrderRepository,
    OrderRepository>();

builder.Services.AddScoped<
    IServiceOrder,
    ServiceOrder>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseGlobalExceptionHandler();

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
