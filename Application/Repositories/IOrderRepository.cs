namespace Application.Repositories
{
    public interface IOrderRepository
    {
        Task<Domain.Order> AddAsync(Domain.Order order);
        Task<Domain.Order?> GetOrderByIdAsync(long orderId);
        Task<IEnumerable<Domain.Order>?> GetAllAsync();
        Task<Domain.Order> UpdateAsync(Domain.Order order);
    }
}
