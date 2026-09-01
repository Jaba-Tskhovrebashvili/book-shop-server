using FirstProject.Data;
using FirstProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace FirstProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private PRODUCT_PKG _product_pkg;
        private readonly ILogger<ProductController> _logger;
        public ProductController(PRODUCT_PKG product_pkg, ILogger<ProductController> logger) { 
        this._product_pkg = product_pkg;
        this._logger = logger;
        }

        [HttpPost("add-product")]
        [Authorize(Roles = "Admin")]
        async public Task<IActionResult> AddProduct(AddProduct product)
        {
            try
            {
                await this._product_pkg.AddProductFunct(product);
                return StatusCode(200, new { success = true, message = "პროდუქტი წარმატებით დაემატა" });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { message = ex.Message, success = false });
            }
        }

        [HttpPut("add-product/{productId}")]
        [Authorize(Roles = "Admin")]
        async public Task<IActionResult> EditProduct(AddProduct product, int productId)
        {
            try
            {
                await this._product_pkg.EditProductFunct(product, productId);
                return StatusCode(200, new { success = true, message = "პროდუქტის რედაქტირება წარმატებულია" });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { message = ex.Message, success = false });
            }
        }

        [HttpDelete("delete-product/{productId}")]
        [Authorize(Roles = "Admin")]
        async public Task<IActionResult> DeleteProduct(int productId)
        {
            try
            {
                await this._product_pkg.DeleteProductFunct(productId);
                return StatusCode(200, new { success = true, message = "პროდუქტის წაიშალა" });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { message = ex.Message, success = false });
            }
        }
    }
}
