using Application.DTos.Product;
using Application.DTos;

namespace Application.Product;

public interface IProductService
{
    Task<BaseResponse<List<ProductDto>>> GetProductsAsync();
    
}
