using Application.DTos.Order;
using Application.Exceptions;
using Application.Order;
using Application.Payment;
using Application.Payment.Models;
using Application.Repositories;
using Domain;
using Domain.Enums;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Order
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IPaymentService _paymentService;
        private readonly IConfiguration _configuration;
        private readonly string apiUserId;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository, IPaymentService paymentService, IConfiguration configuration)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _paymentService = paymentService;
            _configuration = configuration;
            apiUserId = _configuration.GetValue<string>("ApiUrls:UserId");
        }

        public async Task<OrderDto> CompleteOrder(long orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);

            if (order is null)
            {
                throw new OrderException<OrderDto>("Sipariş bulunamadı");
            }

            switch (order.OrderStatus)
            {
                case OrderStatus.NewOrder:
                    throw new OrderException<OrderDto>("Ön sipariş bekleniyor");
                    break;
                case OrderStatus.Complete:
                    throw new OrderException<OrderDto>("Sipariş daha önce tamamlanmıştır.");
                    break;
                case OrderStatus.Cancelled:
                    throw new OrderException<OrderDto>("İptal edilmiş sipariş için siparişi tamamlama yapılamamaktadır");
                    break;
            }

            var completeResult = await _paymentService.CompletePaymentAsync(orderId);

            if (completeResult)
            {
                order.OrderStatus = OrderStatus.Complete;
                await _orderRepository.UpdateAsync(order);

                return new OrderDto()
                {
                    Id = order.Id,
                    UserId = apiUserId,
                    OrderStatusId = order.StatusId,
                    Total = order.TotalPrice,
                    OrderStatus = order.OrderStatus.ToString(),
                    Items = order.Items.Select(x => new OrderItemDto()
                    {
                        Id = x.Id,
                        Total = x.Total,
                        ProductCode = x.Product.Code,
                        Quantity = x.Quantity
                    }).ToList(),
                };
            }

            return new OrderDto()
            {
                Id = order.Id,
                UserId = apiUserId,
                OrderStatusId = order.StatusId,
                Total = order.TotalPrice,
                OrderStatus = order.OrderStatus.ToString(),
                Items = order.Items.Select(x => new OrderItemDto()
                {
                    Id = x.Id,
                    UserId = apiUserId,
                    Total = x.Total,
                    ProductCode = x.Product.Code,
                    Quantity = x.Quantity
                }).ToList(),
            };
        }

        public async Task<OrderDto> CreateOrder(OrderCreateDto orderCreateDto)
        {
            var productCodes = orderCreateDto.Items.Select(x => x.ProductCode).ToList();

            var existingProducts = await _productRepository.GetAllAsync();

            if (existingProducts is null)
            {
                throw new OrderException<OrderDto>($"Ürün bulunamadı");
            }

            var existingProductCodes = existingProducts.Select(x => x.Code).ToList();

            var notExists = productCodes.Except(existingProductCodes).ToList();

            if (notExists?.Count > 0)
            {
                throw new OrderException<OrderDto>($"Geçersiz Ürün Kodu: {string.Join(",", notExists)}");
            }

            var totalPrice = existingProducts.Where(x => productCodes.Contains(x.Code))
                .Select(x => x.Price * orderCreateDto.Items.Where(y => y.ProductCode == x.Code).FirstOrDefault().Quantity)
                .Sum();

            var availableBalance = await _paymentService.GetAvailableBalanceAsync();

            if (totalPrice > availableBalance)
            {
                throw new OrderException<OrderDto>("Yetersiz bakiye");
            }


            var order = new Domain.Order()
            {
                Description = orderCreateDto.Description,
                TotalPrice = totalPrice,
                OrderStatus = OrderStatus.NewOrder,
                UserId = apiUserId,
                Items = orderCreateDto.Items.Select(x => new OrderItem()
                {
                    UserId = apiUserId,
                    Total = x.Quantity * existingProducts.FirstOrDefault(p => p.Code == x.ProductCode).Price,
                    ProductId = existingProducts.FirstOrDefault(p => p.Code == x.ProductCode).Id,
                    Quantity = x.Quantity,
                    Currency = existingProducts?.FirstOrDefault(p => p.Code == x.ProductCode)?.Currency,

                }).ToList()
            };

            await _orderRepository.AddAsync(order);

            ProcessPaymentCreateModel paymentCreateModel = new()
            {
                Amount = totalPrice
            };

            var paymentResult = await _paymentService.StartPaymentAsync(paymentCreateModel);

            if (paymentResult.Status == PaymentStatus.Blocked)
            {
                order.OrderStatus = OrderStatus.PreOrder;
                await _orderRepository.UpdateAsync(order);
            }

            var orderDto = new OrderDto()
            {
                Id = order.Id,
                Items = order.Items.Select(x => new OrderItemDto()
                {
                    Id = x.Id,
                    Total = x.Total,
                    ProductCode = x.Product.Code,
                    Quantity = x.Quantity
                }).ToList(),
            };

            return orderDto;
        }

        public async Task<OrderDto?> GetOrderById(long orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);

            if (order is null)
            {
                throw new OrderException<OrderDto>("Sipariş bulunamadı!");
            }

            return new OrderDto()
            {
                Id = order.Id,
                Items = order.Items.Select(x => new OrderItemDto()
                {
                    Id = x.Id,
                    Total = x.Total,
                    ProductCode = x.Product.Code,
                    Quantity = x.Quantity
                }).ToList(),
            };
        }

        public async Task<IEnumerable<OrderDto>?> GetOrders()
        {
            var orders = await _orderRepository.GetAllAsync();
            if (orders is null)
            {
                throw new OrderException<IEnumerable<OrderDto>>("Siparişler listelenemedi!");
            }

            var result = orders.Select(order => new OrderDto()
            {
                Id = order.Id,
                OrderStatusId = order.StatusId,
                Total = order.TotalPrice,
                Items = order.Items.Select(item => new OrderItemDto()
                {
                    Id = item.Id,
                    Total = item.Total,
                    ProductCode = item.Product.Code,
                    Quantity = item.Quantity
                }).ToList()
            }).ToList();

            return result;
        }
    }
}
