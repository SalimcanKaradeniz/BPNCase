using Application.Order;
using Application.Payment;
using Application.Product;
using Infrastructure.Services.Order;
using Infrastructure.Services.Payment;
using Infrastructure.Services.Product;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace Infrastructure;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IProductService, ProductService>();

        var apiUrl = configuration.GetSection("ApiUrls:BaseUrl")?.Value;

        if (apiUrl != null)
        {
            services.AddHttpClient("apiClient", client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
                client.BaseAddress = new Uri(apiUrl);
            }).AddPolicyHandler(HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (outcome, timespan, retryAttempt, context) =>
                {
                    Console.WriteLine($"[{DateTime.Now}] Retry {retryAttempt} - Waiting {timespan.TotalSeconds} seconds");
                }));
        }

        return services;
    }
}