using Microsoft.AspNetCore.Mvc;
using PickupExpress.API.Models;
using PickupExpress.Core.DTOs;
using PickupExpress.Core.Interfaces;
using PickupExpress.Core.Models;

namespace PickupExpress.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // GET: api/order
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetOrdersByStatus(OrderStatus status)
        {
            var orders = await _orderRepository.GetOrdersByStatusAsync(status);
            return Ok(orders);
        }

        // GET: api/order/pending
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingOrders()
        {
            var orders = await _orderRepository.GetPendingOrdersAsync();
            return Ok(orders);
        }

        // GET: api/order/completed
        [HttpGet("completed")]
        public async Task<IActionResult> GetCompletedOrders()
        {
            var orders = await _orderRepository.GetCompletedOrdersAsync();
            return Ok(orders);
        }

        // GET: api/order/cancelled
        [HttpGet("cancelled")]
        public async Task<IActionResult> GetCancelledOrders()
        {
            var orders = await _orderRepository.GetCancelledOrdersAsync();
            return Ok(orders);
        }

        // GET: api/order/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrdersByUserId(int userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);

            if (orders == null || !orders.Any())
                return NotFound($"No orders under user {userId}");

            return Ok(orders);
        }


        // GET: api/order/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);

            if (order == null)
            {
                return NotFound(new { message = $"Order with ID {id} not found" });
            }

            return Ok(order);
        }

        // POST: api/order
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto
        dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (dto.OrderItems == null || dto.OrderItems.Count == 0)
            {
                return BadRequest("Order must contain at least one item.");
            }

            var order = new Order
            {
                CustomerId = dto.CustomerId,
                Status = dto.Status,
                OrderDate = dto.OrderDate,
                OrderItems = dto.OrderItems.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                }).ToList()
            };

            var createdOrder = await _orderRepository.CreateOrderAsync(order);

            return CreatedAtAction(
                nameof(GetOrderById),
                new { id = createdOrder.OrderId },
                createdOrder
            );
        }

        // PATCH: api/order/{id}/status
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] OrderStatusUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var exists = await _orderRepository.ExistsAsync(id);
            if (!exists)
            {
                return NotFound(new { message = $"Order with ID {id} not found" });
            }

            var updatedOrder = await _orderRepository.UpdateOrderStatusAsync(id, dto.NewStatus);

            return Ok(updatedOrder);
        }

        // DELETE: api/order/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var deleted = await _orderRepository.DeleteOrderAsync(id);

            if (!deleted)
            {
                return NotFound(new { message = $"Order with ID {id} not found" });
            }

            return NoContent();
        }
    }
}