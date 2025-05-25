using Microsoft.EntityFrameworkCore;
using Application;
using Domain;
using Application.Exceptions;
using Application.Repositories;

namespace Persistence.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _dbContext;

        public ProductRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Products> AddAsync(Products product)
        {
            await _dbContext.Products.AddAsync(product);
            var isSaved = await _dbContext.SaveChangesAsync() > 0;
            return isSaved ? product : throw new Exception();
        }

        public async Task<IEnumerable<Products>> AddRangeAsync(IEnumerable<Products> products)
        {
            var productList = products.ToList();
            await _dbContext.Products.AddRangeAsync(productList);
            var isSaved = await _dbContext.SaveChangesAsync() > 0;
            return isSaved ? productList : throw new Exception();

        }

        public async Task<Products?> GetProductByIdAsync(long productId)
        {
            var product = await _dbContext.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == productId);

            if (product is null)
            {
                throw new EntityNotFoundException<Products>(typeof(Products).Name, productId);
            }

            return product;
        }

        public async Task<List<Products>?> GetAllAsync()
        {
            var products = await _dbContext.Products.AsNoTracking().ToListAsync();

            if (products is null)
            {
                throw new EntityNotFoundException<Products>(typeof(Products).Name);
            }

            return products;
        }
    }
}
