
using Comment.API.IntegrationEvents;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Post.Api.Models;
using System.Text.Json;

[ApiController]
[Route("api/[controller]")]
public class PostController(HttpClient _httpClient, AppDbContext _dbContext, IPublishEndpoint _publishEndpoint) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] PostDto dto)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        await _dbContext.Posts.AddAsync(dto);

        // Call the feed service to update the feeds
        //await _httpClient.PostAsJsonAsync("https://feed-service/api/feed/update", dto);

        var postCreatedEvent = new PostCreatedEvent
        {
            Id = Guid.NewGuid(),
            Content = dto.Content,
            PostId = dto.PostId,
            UserId = dto.UserId,
        };

        var outboxMessage = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            IsProcessed = false,
            Payload = JsonSerializer.Serialize(postCreatedEvent),
            CreatedAt = DateTime.UtcNow
        };
        await _dbContext.OutboxMessages.AddAsync(outboxMessage);
        await _dbContext.SaveChangesAsync();
        await transaction.CommitAsync();


        //await _publishEndpoint.Publish(postCreatedEvent);

        return CreatedAtAction(nameof(CreatePost), new { id = dto.Id }, dto);
    }
}
