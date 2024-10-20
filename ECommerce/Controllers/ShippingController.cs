using Microsoft.AspNetCore.Mvc;

namespace Customer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingController(ShippingDbContext context) : ControllerBase
    {
        [HttpPost("ship-order")]
        public async Task<IActionResult> ShipOrder([FromBody] ShippingDto shippingDto)
        {
            var shipment = new Shipment
            {
                OrderId = shippingDto.OrderId,
                ShippingDate = DateTime.UtcNow,
                TrackingNumber = "XYZ123"
            };

            context.Shipments.Add(shipment);
            await context.SaveChangesAsync();
            return Ok(shipment);
        }
    }
}
