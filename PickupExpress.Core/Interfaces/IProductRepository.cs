using PickupExpress.Core.Models;

namespace PickupExpress.Core.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<IEnumerable<Product>> GetAvailableProductsAsync();
        Task<Product?> GetProductByIdAsync(int productId);

        Task<Product> CreateProductAsync(Product product);
        Task<Product?> UpdateProductAsync(Product product);
        Task<Product?> UpdateStockQuantityAsync(int productId, int newQuantity);
        Task<bool> DeleteProductAsync(int productId);

        Task<bool> ExistsAsync(int productId);
    }
}