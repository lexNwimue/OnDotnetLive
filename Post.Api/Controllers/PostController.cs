
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PostController(HttpClient _httpClient, AppDbContext _dbContext) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] PostDto dto)
    {
        await _dbContext.Posts.AddAsync(dto);
        await _dbContext.SaveChangesAsync();

        // Call the feed service to update the feeds
        await _httpClient.PostAsJsonAsync("https://feed-service/api/feed/update", dto);

        return CreatedAtAction(nameof(CreatePost), new { id = dto.Id }, dto);
    }
}
