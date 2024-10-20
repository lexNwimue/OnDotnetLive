using Microsoft.AspNetCore.Mvc;

namespace Customer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController(PaymentDbContext context) : ControllerBase
    {

        [HttpPost("process-payment")]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentDto paymentDto)
        {
            var payment = new Payment
            {
                OrderId = paymentDto.OrderId,
                Amount = paymentDto.Amount,
                PaymentDate = DateTime.UtcNow
            };

            context.Payments.Add(payment);
            await context.SaveChangesAsync();
            return Ok(payment);
        }
    }
}
