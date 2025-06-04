using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_food_delivery_system.Models;
using Online_food_delivery_system.Service;

namespace Online_food_delivery_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowReactApp")]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentService _paymentService;
        public PaymentController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // Get: api/Payment
        [HttpGet]
        public async Task<IActionResult> GetAllPayments()
        {
            var payments = await _paymentService.GetAllPaymentsAsync();
            return Ok(payments);
        }
        [HttpGet("{id}/search")]
        public async Task<IActionResult> GetPaymenttByOrderId(int id)
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);
            if (payment == null)
                return NotFound("Payment not found");
            return Ok(payment);
        }
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdatePaymentStatus(int id, [FromBody] string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return BadRequest("Status cannot be null or empty");

            string normalizedStatus = status.ToLower();

            if (normalizedStatus != "completed")
                return BadRequest("Only 'completed' status is allowed for this operation.");

            var payment = await _paymentService.GetPaymentByIdAsync(id);
            if (payment == null)
                return NotFound("Payment not found");

            payment.Status = "Completed";
            payment.PaymentTime = DateTime.Now;
            await _paymentService.UpdatePaymentAsync(payment);

            return Ok("Payment status updated successfully");
        }



        //// Post: api/Payment
        //[HttpPost]
        //public async Task<IActionResult> AddPayment([FromBody] Payment payment)
        //{
        //    if (payment == null) return BadRequest("Payment cannot be null");
        //    await _paymentService.AddPaymentAsync(payment);
        //    return CreatedAtAction(nameof(GetPaymentById), new { id = payment.PaymentID }, payment);
        //}

        // Put: api/Payment/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(int id, [FromBody] Payment payment)
        {
            if (id != payment.PaymentID) return BadRequest("Payment ID mismatch");
            await _paymentService.UpdatePaymentAsync(payment);
            return NoContent();
        }

        // Delete: api/Payment/{id}
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeletePayment(int id)
        //{
        //    await _paymentService.DeletePaymentAsync(id);
        //    return NoContent();
        //}

        //// Post: api/Payment/Process
        //[HttpPost("Process")]
        //public async Task<IActionResult> ProcessPayment([FromBody] Payment payment)
        //{
        //    if (payment == null) return BadRequest("Payment cannot be null");
        //    var processedPayment = await _paymentService.ProcessPaymentAsync(payment);
        //    return Ok(processedPayment);
        //}
    }
}
