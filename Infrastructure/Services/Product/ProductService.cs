using Application.DTos.Product;
using Application.Product;
using Application.Repositories;
using Domain;
using Infrastructure.Services.Product.Models;
using System.Collections.Generic;
using System.Net.Http.Json;

namespace Infrastructure.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly IProductRepository _productRepository;
        public ProductService(IHttpClientFactory httpClientFactory, IProductRepository productRepository)
        {
            _httpClient = httpClientFactory.CreateClient("apiClient");
            _productRepository = productRepository;
        }

        public async Task<Application.DTos.BaseResponse<List<ProductDto>>> GetProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();

            if (products?.Count() > 0)
            {
                return new()
                {
                    IsSuccess = true,
                    Data = products.Select(productResponse =>
                    new ProductDto()
                    {
                        Code = productResponse.Code,
                        Name = productResponse.Name,
                        Description = productResponse.Description,
                        Category = productResponse.Category,
                        Price = productResponse.Price,
                        Stock = productResponse.Stock,
                        Currency = productResponse.Currency
                    }).ToList()
                };
            }

            var serviceResult = await _httpClient.GetFromJsonAsync<BaseResponse<List<ProductModel>>>("api/products");

            if (serviceResult?.Data == null)
            {
                return new()
                {
                    IsSuccess = false,
                    ErrorMessage = "Ürün bulunmamaktadır",
                    Data = [],
                };
            }

            var productDtos = serviceResult.Data.Select(
                productResponse =>
                new ProductDto()
                {
                    Code = productResponse.Id,
                    Name = productResponse.Name,
                    Description = productResponse.Description,
                    Category = productResponse.Category,
                    Price = productResponse.Price,
                    Stock = productResponse.Stock,
                    Currency = productResponse.Currency
                }
            ).ToList();

            var productEntities = serviceResult.Data.Select(
                productResponse =>
                new Products()
                {
                    Name = productResponse.Name,
                    Description = productResponse.Description,
                    Price = productResponse.Price,
                    Code = productResponse.Id,
                    Stock = productResponse.Stock,
                    Category = productResponse.Category,
                    Currency = productResponse.Currency,
                }).ToList();

            await _productRepository.AddRangeAsync(productEntities);

            Application.DTos.BaseResponse<List<ProductDto>> result = new()
            {
                IsSuccess = true,
                Data = productDtos,
            };

            return result;
        }
    }
}
