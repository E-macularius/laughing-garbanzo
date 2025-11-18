using InventoryManagement.Application.DTOs;
using InventoryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Api.Controllers
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

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            // This hits the /api/products/search endpoint if query params are present
            // This is a check to redirect to the more powerful search endpoint
            if (Request.Query.Count > 0)
            {
                return RedirectToAction(nameof(SearchProducts));
            }
            
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("search")] // GET /api/products/search
        public async Task<IActionResult> SearchProducts([FromQuery] ProductSearchQueryDto queryDto)
        {
            var result = await _productService.SearchProductsAsync(queryDto);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto createDto)
        {
            var newProduct = await _productService.CreateProductAsync(createDto);
            return CreatedAtAction(nameof(GetProduct), new { id = newProduct.Id }, newProduct);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, UpdateProductDto updateDto)
        {
            var success = await _productService.UpdateProductAsync(id, updateDto);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var success = await _productService.DeleteProductAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}