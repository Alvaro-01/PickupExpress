using Microsoft.EntityFrameworkCore;
using PickupExpress.Core.Interfaces;
using PickupExpress.Core.Models;
using PickupExpress.Infrastructure.Data;

namespace PickupExpress.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAvailableProductsAsync()
        {
            return await _context.Products.Where(p =>
                    p.QuantityInStock > 0).ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            return await _context.Products.FindAsync(productId);
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> UpdateProductAsync(Product product)
        {
            var existingProduct = await _context.Products.FindAsync(product.ProductId);
            if (existingProduct == null)
            {
                return null;
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.QuantityInStock = product.QuantityInStock;

            await _context.SaveChangesAsync();
            return existingProduct;
        }

        public async Task<Product?> UpdateStockQuantityAsync(int productId, int newQuantity)
        {
            var existingProduct = await _context.Products.FindAsync(productId);
            if (existingProduct == null)
            {
                return null;
            }

            existingProduct.QuantityInStock = newQuantity;

            await _context.SaveChangesAsync();
            return existingProduct;
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return false;
            }

            _context.Products.Remove(product);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsAsync(int productId)
        {
            return await _context.Products.AnyAsync(p => p.ProductId == productId);
        }
    }
}