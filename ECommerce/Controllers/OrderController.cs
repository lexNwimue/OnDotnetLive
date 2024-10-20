using Microsoft.AspNetCore.Mvc;

namespace Customer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(OrderDbContext context) : ControllerBase
    {
        [HttpPost("place-order")]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderDto orderDto)
        {
            var order = new Order
            {
                CustomerId = orderDto.CustomerId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = orderDto.TotalAmount
            };

            context.Orders.Add(order);
            await context.SaveChangesAsync();
            return Ok(order);
        }
    }
}
