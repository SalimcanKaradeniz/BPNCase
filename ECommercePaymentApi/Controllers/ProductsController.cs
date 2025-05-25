using Application.Product;
using Microsoft.AspNetCore.Mvc;

namespace ECommercePaymentApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Tüm ürünleri listeleyen api ucudur.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await _productService.GetProductsAsync();
            return Ok(products);
        }
    }
}
