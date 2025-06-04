using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Online_food_delivery_system.Models;
using Online_food_delivery_system.Service;

namespace Online_food_delivery_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowReactApp")]
    public class DeliveryController : ControllerBase
    {
        private readonly DeliveryService _deliveryService;

        public DeliveryController(DeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDeliveries()
        {
            var deliveries = await _deliveryService.GetAllDeliveriesAsync();
            return Ok(deliveries);
        }
        [HttpPost("assign/auto")]
        public async Task<IActionResult> AssignDeliveryAgentAutomatically(int orderId, DateTime estimatedTimeOfArrival)
        {
            try
            {
                await _deliveryService.AssignDeliveryAgentAutomaticallyAsync(orderId, estimatedTimeOfArrival);
                return Ok("Delivery agent assigned automatically and successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("assign")]
        public async Task<IActionResult> AssignDeliveryAgent(int orderId, int agentId, DateTime estimatedTimeOfArrival)
        {
            await _deliveryService.AssignDeliveryAgentAsync(orderId, agentId, estimatedTimeOfArrival);
            return Ok("Delivery agent assigned successfully");
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateDeliveryStatus(int id, [FromBody] string status)
        {
            await _deliveryService.UpdateDeliveryAsync(id, status);
            return Ok("Delivery status updated successfully");
        }
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> PatchDeliveryStatus(int id, [FromBody] string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return BadRequest("Status cannot be null or empty");

            if (status.ToLower() != "delivered")
                return BadRequest("Only 'completed' status is allowed for this operation.");

            try
            {
                var delivery = await _deliveryService.GetDeliveryByIdAsync(id);
                if (delivery == null)
                    return NotFound("Delivery not found");

                delivery.Status = "Completed";

                if (delivery.Agent != null)
                {
                    delivery.Agent.IsAvailable = true;
                    await _deliveryService.UpdateAgentAsync(delivery.Agent);
                }

                await _deliveryService.UpdateDeliveryAsync(delivery.DeliveryID,status);
                return Ok("Delivery status updated to 'completed' and agent marked as available.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
