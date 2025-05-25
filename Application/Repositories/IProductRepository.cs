using Domain;

namespace Application.Repositories;

public interface IProductRepository
{
    Task<Products> AddAsync(Products product);
    Task<IEnumerable<Products>> AddRangeAsync(IEnumerable<Products> products);
    Task<Products?> GetProductByIdAsync(long productId);
    Task<List<Products>?> GetAllAsync();
}
