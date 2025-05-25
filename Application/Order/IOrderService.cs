using Application.DTos.Order;

namespace Application.Order
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrder(OrderCreateDto orderCreateDto);
        Task<OrderDto> CompleteOrder(long orderId);
        Task<OrderDto?> GetOrderById(long orderId);
        Task<IEnumerable<OrderDto>?> GetOrders();
    }
}
