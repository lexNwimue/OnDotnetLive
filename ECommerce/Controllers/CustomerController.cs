using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly CustomerDbContext _context;

    public CustomerController(CustomerDbContext context)
    {
        _context = context;
    }

    [HttpPost("add-customer")]
    public async Task<IActionResult> AddCustomer([FromBody] CustomerDto customerDto)
    {
        var customer = new Customer
        {
            Name = customerDto.Name,
            Email = customerDto.Email,
            Address = customerDto.Address
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return Ok(customer);
    }
}
