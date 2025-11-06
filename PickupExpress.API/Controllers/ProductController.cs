using Microsoft.AspNetCore.Mvc;
using PickupExpress.API.Models;
using PickupExpress.Core.DTOs;
using PickupExpress.Core.Interfaces;
using PickupExpress.Core.Models;

namespace PickupExpress.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // GET: api/product
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productRepository.GetAllProductsAsync();
            return Ok(products);
        }

        // GET: api/product/available
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableProducts()
        {
            var products = await _productRepository.GetAvailableProductsAsync();
            return Ok(products);
        }

        // GET: api/product/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound(new { message = $"Product with ID {id} not found" });
            }

            return Ok(product);
        }

        // POST: api/product
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDto
        dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                QuantityInStock = dto.QuantityInStock
            };

            var createdProduct = await _productRepository.CreateProductAsync(product);

            return CreatedAtAction(
                nameof(GetProductById),
                new { id = createdProduct.ProductId },
                createdProduct
            );
        }

        // PUT api/product/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var exists = await _productRepository.ExistsAsync(id);
            if (!exists)
            {
                return NotFound(new { message = $"Product with ID {id} not found" });
            }

            var product = new Product
            {
                ProductId = id,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                QuantityInStock = dto.QuantityInStock
            };

            var updatedProduct = await _productRepository.UpdateProductAsync(product);

            return Ok(updatedProduct);
        }

        // Using DTO since only stock quantity is being updated
        // PATCH: api/product/{id}/quantity
        [HttpPatch("{id}/quantity")]
        public async Task<IActionResult> UpdateQuantityInStock(int id, [FromBody] ProductStockUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var exists = await _productRepository.ExistsAsync(id);
            if (!exists)
            {
                return NotFound(new { message = $"Product with ID {id} not found" });
            }

            var updatedProduct = await _productRepository.UpdateStockQuantityAsync(id, dto.NewQuantity);

            return Ok(updatedProduct);
        }

        // DELETE: api/product/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var deleted = await _productRepository.DeleteProductAsync(id);

            if (!deleted)
            {
                return NotFound(new { message = $"Product with ID {id} not found" });
            }

            return NoContent();
        }
    }
}