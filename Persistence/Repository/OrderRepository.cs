using Application.Exceptions;
using Application.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _dbContext;

        public OrderRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Order> AddAsync(Order order)
        {
            var strategy = _dbContext.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    await _dbContext.Orders.AddAsync(order);
                    var isSaved = await _dbContext.SaveChangesAsync() > 0;

                    if (!isSaved)
                    {
                        await transaction.RollbackAsync();
                        throw new OrderException<Order>("Sipariş oluşturulurken hata oluştu!");
                    }

                    await transaction.CommitAsync();

                    return await _dbContext.Orders
                        .Include(o => o.Items).ThenInclude(p => p.Product)
                        .FirstAsync(o => o.Id == order.Id);
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();

                    throw new OrderException<Order>("Sipariş oluşturulurken hata oluştu!");
                }
            });
        }

        public async Task<IEnumerable<Order>?> GetAllAsync()
        {
            return await _dbContext.Orders.Include(x => x.Items).ThenInclude(p => p.Product).AsNoTracking().ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(long orderId)
        {
            return await _dbContext.Orders.Include(i => i.Items).ThenInclude(p => p.Product).AsNoTracking().FirstOrDefaultAsync(x => x.Id == orderId);
        }

        public async Task<Order> UpdateAsync(Order order)
        {
            var existingOrder = await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == order.Id);

            if (existingOrder is null)
            {
                throw new Exception();
            }

            existingOrder.Description = order.Description;
            existingOrder.OrderStatus = order.OrderStatus;

            var isUpdated = await _dbContext.SaveChangesAsync() > 0;
            return isUpdated ? existingOrder : throw new Exception();
        }
    }
}
