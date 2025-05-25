using Application.DTos;
using Application.DTos.Order;
using Application.DTos.Payment;
using Application.Exceptions;
using Application.Payment;
using Application.Payment.Models;
using Domain.Enums;
using Infrastructure.Services.Product.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Json;

namespace Infrastructure.Services.Payment
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string apiUserId;

        public PaymentService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient("apiClient");
            _configuration = configuration;
            apiUserId = _configuration.GetValue<string>("ApiUrls:UserId");
        }
        
        public async Task<bool> CompletePaymentAsync(long orderId)
        {
            CompleteOrderRequestDto completeOrderRequestDto = new()
            {
                OrderId = apiUserId,
            };

            var httpResponse = await _httpClient.PostAsJsonAsync("api/balance/complete", completeOrderRequestDto);

            if (httpResponse.IsSuccessStatusCode)
            {
                var serviceResult = await httpResponse.Content.ReadFromJsonAsync<BaseResponse<CompleteOrderModel>>();

                if (serviceResult.Success && serviceResult.Data.Order.Status == "completed")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                throw new OrderException<object>("sipariş tamamlanamadı");
            }
        }

        public async Task<decimal> GetAvailableBalanceAsync()
        {
            var serviceResult = await _httpClient.GetFromJsonAsync<BaseResponse<BalanceDto>>("api/balance");

            if (serviceResult == null)
            {
                throw new OrderException<PreOrderResponseDto>("Bakiye kontrolü esnasında hata oluştu lütfen daha sonra tekrar deneyiniz");
            }

            return serviceResult?.Data?.AvailableBalance?? 0;
        }

        public async Task<ProcessPaymentModel> StartPaymentAsync(ProcessPaymentCreateModel processPaymentCreateModel)
        {
            PreOrderRequestDto preOrderRequestModel = new()
            {
                Amount = processPaymentCreateModel.Amount,
                OrderId = apiUserId,
            };

            var httpResponse = await _httpClient.PostAsJsonAsync("api/balance/preorder", preOrderRequestModel);


            if (httpResponse.IsSuccessStatusCode)
            {
                var serviceResult = await httpResponse.Content.ReadFromJsonAsync<BaseResponse<PreOrderResponse>>();

                if (serviceResult.Success && serviceResult.Data.PreOrder.Status == "blocked")
                {
                    return new ProcessPaymentModel()
                    {
                        Status = PaymentStatus.Blocked
                    };
                }
                else
                {
                    return new ProcessPaymentModel()
                    {
                        Status = PaymentStatus.Failed
                    };
                }
            }
            else
            {
                throw new OrderException<PreOrderResponseDto>("Sipariş başlatılamadı!");
            }
        }
    }
}
