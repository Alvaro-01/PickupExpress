using PickupExpress.Core.Models;

namespace PickupExpress.Core.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status);
        Task<IEnumerable<Order>> GetPendingOrdersAsync();
        Task<IEnumerable<Order>> GetCompletedOrdersAsync();
        Task<IEnumerable<Order>> GetCancelledOrdersAsync();
        Task<List<Order>> GetOrdersByUserIdAsync(int userId);
        Task<Order?> GetOrderByIdAsync(int orderId);

        Task<Order> CreateOrderAsync(Order order);
        Task<Order?> UpdateOrderStatusAsync(int orderId, OrderStatus newOrderStatus);
        Task<Order?> UpdateOrderNotesAsync(int orderId, string notes);
        Task<bool> DeleteOrderAsync(int orderId);

        Task<bool> ExistsAsync(int orderId);
    }
}