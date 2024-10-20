
using Comment.API.IntegrationEvents;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CommentController(HttpClient _httpClient, AppDbContext _dbContext, IPublishEndpoint _publishEndpoint) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateComment([FromBody] CommentDto dto)
    {
        await _dbContext.Comments.AddAsync(dto);
        await _dbContext.SaveChangesAsync();

        // Call the feed service to update the feeds
        //await _httpClient.PostAsJsonAsync("https://feed-service/api/feed/update", dto);

        var commentCreatedEvent = new CommentCreatedEvent
        {
            Id = Guid.NewGuid(),
            CommentId = dto.CommentId,
            Content = dto.Content,
            PostId = dto.PostId,
            UserId = dto.UserId,
        };
        await _publishEndpoint.Publish(commentCreatedEvent);


        return CreatedAtAction(nameof(CreateComment), new { id = dto.Id }, dto);
    }
}
