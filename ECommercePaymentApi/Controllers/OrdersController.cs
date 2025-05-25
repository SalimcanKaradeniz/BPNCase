using Application.DTos;
using Application.DTos.Order;
using Application.Order;
using Azure;
using Microsoft.AspNetCore.Mvc;

namespace ECommercePaymentApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Tüm sipariþleri listeleyen api ucudur.
        /// </summary>
        /// <returns></returns>
        [HttpGet("orders")]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderService.GetOrders();
            return Ok(orders);
        }

        /// <summary>
        /// Sipariþ detayýnýn gösterimini saðlayan api ucudur.
        /// </summary>
        /// <returns></returns>
        [HttpGet("order/{id}")]
        public async Task<IActionResult> GetOrderById(long id)
        {
            var orders = await _orderService.GetOrderById(id);
            return Ok(orders);
        }

        /// <summary>
        /// Ön sipariþ oluþturan api ucudur.(PreOrder) 
        /// </summary>
        /// <param name="createOrderDto"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto orderCreateDto)
        {
            var order = await _orderService.CreateOrder(orderCreateDto);

            if (order != null && order.Id > 0)
                return Ok(new BaseResponse<OrderDto>()
                {
                    IsSuccess = true,
                    Data = order,
                });

            return Ok(new BaseResponse<OrderDto>()
            {
                IsSuccess = false,
                Data = null,
            });

        }

        /// <summary>
        /// Ön sipariþi tamamlayýp sipariþi oluþturan api ucudur.
        /// id deðeri olarak oluþan sipariþ kayýtýna ait id deðeri gönderilmelidir.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("complete/{id}")]
        public async Task<IActionResult> CompleteOrder(long id)
        {
            var order = await _orderService.CompleteOrder(id);

            return Ok(order);
        }
    }
}
