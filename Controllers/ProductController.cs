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

        [HttpGet("author-products")]
       //[Authorize(Roles = "Admin")]
        async public Task<IActionResult> AuthorProducts(int authorId,int page)
        {
            try
            {
                var products=await this._product_pkg.AuthorProducts(authorId, page);
                return StatusCode(200, new { success = true, products });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { message = ex.Message, success = false });
            }
        }

        [HttpGet("get-products")]
        //[Authorize(Roles = "Admin")]
        async public Task<IActionResult> GetProducts(int? typeId,int? publishId,string? search, int page)
        {
            try
            {
                var products = await this._product_pkg.GetProducts(typeId, publishId, search, page);
                return StatusCode(200, new { success = true, products });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { message = ex.Message, success = false });
            }
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

        [HttpPut("edit-product/{productId}")]
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


        [HttpGet("get-product-types")]
        public async Task<IActionResult> GetProductTypes(string? search)
        {
            try
            {

                var Product_types = await this._product_pkg.GetProducttypes(search);
                return StatusCode(200, new { success = true, Product_types });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { message = ex.Message, success = false });
            }
        }

        [HttpGet("get-publishing-houses")]
        public async Task<IActionResult> GetPublishingHouse(string? search,int page)
        {
            try
            {

                var publishing_houses = await this._product_pkg.GetPublishingHouse(search, page);
                return StatusCode(200, new { success = true, publishing_houses });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { message = ex.Message, success = false });
            }
        }

    }
}
