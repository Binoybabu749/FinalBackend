using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Online_food_delivery_system.Migrations;
using Online_food_delivery_system.Models;
using Online_food_delivery_system.Service;

namespace Online_food_delivery_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowReactApp")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly MenuItemService _menuItemService;

        public OrderController(OrderService orderService, MenuItemService menuItemService)
        {
            _orderService = orderService;
            _menuItemService = menuItemService;
        }

        // GET: api/Order
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();

            return Ok(orders);
        }

        // GET: api/Order/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        // POST: api/Order
        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] OrderDTO orderDto)
        {
            if (orderDto == null || orderDto.MenuItemIDs == null || !orderDto.MenuItemIDs.Any())
                return BadRequest("Invalid order data. MenuItemIDs must be provided.");

            var menuItems = new List<MenuItem>();
            decimal totalAmount = 0;

            foreach (var itemId in orderDto.MenuItemIDs)
            {
                var item = await _menuItemService.GetMenuItemByIdAsync(itemId);
                if (item == null)
                    return NotFound($"Menu item with ID {itemId} not found.");
                if (item.RestaurantID != orderDto.RestaurantID)
                    return BadRequest($"Menu item with ID {itemId} does not belong to the specified restaurant.");

                menuItems.Add(item);
                totalAmount += item.Price ?? 0;
            }

            var order = new Order
            {
                CustomerID = orderDto.CustomerID,
                RestaurantID = orderDto.RestaurantID,
                Status = orderDto.Status,
                TotalAmount = totalAmount,
                OrderMenuItems = menuItems.Select(mi => new OrderMenuItem
                {
                    ItemID = mi.ItemID
                }).ToList(),
                OrderDate = null
            };

            var createdOrder = await _orderService.AddOrderAsync(order);
            return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.OrderID }, createdOrder);
        }

        // PUT: api/Order/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] Order order)
        {
            if (id != order.OrderID) return BadRequest("Order ID mismatch");
            var updatedOrder = await _orderService.UpdateOrderAsync(order);
            return Ok(updatedOrder);
        }

        // DELETE: api/Order/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return NoContent();
        }
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return BadRequest("Status cannot be null or empty");

            string normalizedStatus = status.ToLower();

            if (normalizedStatus != "confirmed" && normalizedStatus != "completed")
                return BadRequest("Only 'confirmed' or 'completed' statuses are allowed.");

            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound("Order not found");

            Payment payment = order.Payment;

            order.Status = normalizedStatus;

            // Handle "confirmed" status
            if (normalizedStatus == "confirmed")
            {
                if (payment == null)
                {
                    // Create a pending payment if none exists
                    payment = new Payment
                    {
                        OrderID = order.OrderID,
                        Amount = order.TotalAmount,
                        PaymentMethod = "Google Pay",
                        Status = "pending",
                        PaymentTime = null,
                    };
                    await _orderService.CreatePaymentAsync(payment);
                }
                else if (payment.Status?.ToLower() == "completed")
                {
                    // Payment is already completed, so do delivery operation and assign agent
                    var delivery = order.Delivery;
                    if (delivery == null)
                        return BadRequest("Delivery not found for the associated order.");

                    var availableAgent = await _orderService.GetAvailableAgentAsync();
                    if (availableAgent == null)
                        return BadRequest("No available agents to assign.");

                    delivery.AgentID = availableAgent.AgentID;
                    delivery.Agent = availableAgent;
                    delivery.Status = "assigned";

                    availableAgent.IsAvailable = false;

                    await _orderService.UpdateDeliveryAsync(delivery);
                    await _orderService.UpdateAgentAsync(availableAgent);
                }
            }

            // Handle "completed" status
            if (normalizedStatus == "completed")
            {
                if (payment == null || payment.Status?.ToLower() != "completed")
                {
                    return BadRequest("Payment must be completed before completing the order.");
                }

                var delivery = order.Delivery;
                if (delivery == null)
                    return BadRequest("Delivery not found for the associated order.");

                var availableAgent = await _orderService.GetAvailableAgentAsync();
                if (availableAgent == null)
                    return BadRequest("No available agents to assign.");

                delivery.AgentID = availableAgent.AgentID;
                delivery.Agent = availableAgent;
                delivery.Status = "assigned";

                availableAgent.IsAvailable = false;

                await _orderService.UpdateDeliveryAsync(delivery);
                await _orderService.UpdateAgentAsync(availableAgent);
                order.OrderDate = DateTime.Now;
            }

            await _orderService.UpdateOrderAsync(order);
            return Ok("Order status updated successfully");
        }




    }
}