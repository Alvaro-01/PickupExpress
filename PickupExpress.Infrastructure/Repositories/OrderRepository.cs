using Microsoft.EntityFrameworkCore;
using PickupExpress.Core.Interfaces;
using PickupExpress.Core.Models;
using PickupExpress.Infrastructure.Data;


namespace PickupExpress.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status)
        {
            return await _context.Orders
                    .Where(o => o.Status == status)
                    .Include(o => o.Customer)
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetPendingOrdersAsync()
        {
            return await _context.Orders.Where(o =>
                    o.Status == OrderStatus.Pending).ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetCompletedOrdersAsync()
        {
            return await _context.Orders.Where(o =>
                    o.Status == OrderStatus.Completed).ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetCancelledOrdersAsync()
        {
            return await _context.Orders.Where(o =>
                    o.Status == OrderStatus.Cancelled).ToListAsync();
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _context.Orders
                    .Where(o => o.CustomerId == userId)
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .Include(o => o.Customer)
                    .OrderByDescending(o => o.OrderDate) // To show most recent orders first
                    .ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var item in order.OrderItems)
            {
                var product = await _context.Products.FindAsync(item.ProductId);

                if (product == null)
                {
                    throw new Exception($"Product with ID {item.ProductId} not found.");
                }

                if (product.QuantityInStock <= 0)
                {
                    throw new Exception($"{product.Name} is out of stock.");
                }

                if (product.QuantityInStock < item.Quantity)
                {
                    throw new Exception($"Not enough stock for {product.Name}.");
                }

                product.QuantityInStock -= item.Quantity;
            }

            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<Order?> UpdateOrderStatusAsync(int orderId, OrderStatus newOrderStatus)
        {
            var existingOrder = await _context.Orders.FindAsync(orderId);
            if (existingOrder == null)
            {
                return null;
            }

            existingOrder.Status = newOrderStatus;

            await _context.SaveChangesAsync();
            return existingOrder;
        }

        public async Task<Order?> UpdateOrderNotesAsync(int orderId, string notes)
        {
            var existingOrder = await _context.Orders.FindAsync(orderId);
            if (existingOrder == null)
            {
                return null;
            }

            existingOrder.Notes = notes;

            await _context.SaveChangesAsync();
            return existingOrder;
        }


        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return false;
            }

            _context.Orders.Remove(order);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsAsync(int orderId)
        {
            return await _context.Orders.AnyAsync(o => o.OrderId == orderId);
        }
    }
}