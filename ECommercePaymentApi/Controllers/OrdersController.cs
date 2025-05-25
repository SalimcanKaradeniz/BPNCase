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
        /// T�m sipari�leri listeleyen api ucudur.
        /// </summary>
        /// <returns></returns>
        [HttpGet("orders")]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderService.GetOrders();
            return Ok(orders);
        }

        /// <summary>
        /// Sipari� detay�n�n g�sterimini sa�layan api ucudur.
        /// </summary>
        /// <returns></returns>
        [HttpGet("order/{id}")]
        public async Task<IActionResult> GetOrderById(long id)
        {
            var orders = await _orderService.GetOrderById(id);
            return Ok(orders);
        }

        /// <summary>
        /// �n sipari� olu�turan api ucudur.(PreOrder) 
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
        /// �n sipari�i tamamlay�p sipari�i olu�turan api ucudur.
        /// id de�eri olarak olu�an sipari� kay�t�na ait id de�eri g�nderilmelidir.
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
