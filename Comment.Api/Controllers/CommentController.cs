
using Comment.API.IntegrationEvents;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Post.Api.Models;
using System.Text.Json;

[ApiController]
[Route("api/[controller]")]
public class CommentController(HttpClient _httpClient, AppDbContext _dbContext, IPublishEndpoint _publishEndpoint) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateComment([FromBody] CommentDto dto)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        await _dbContext.Comments.AddAsync(dto);

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
        var outboxMessage = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            IsProcessed = false,
            Payload = JsonSerializer.Serialize(commentCreatedEvent),
            CreatedAt = DateTime.UtcNow
        };
        await _dbContext.OutboxMessages.AddAsync(outboxMessage);
        await _dbContext.SaveChangesAsync();
        await transaction.CommitAsync();


        //await _publishEndpoint.Publish(commentCreatedEvent);


        return CreatedAtAction(nameof(CreateComment), new { id = dto.Id }, dto);
    }
}
