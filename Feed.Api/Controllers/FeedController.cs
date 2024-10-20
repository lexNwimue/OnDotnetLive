using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/[controller]")]
public class FeedController(HttpClient _httpClient) : ControllerBase
{
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserFeed(string userId)
    {
        // Create tasks for each service call
        var postsTask = _httpClient.GetAsync($"https://post-service/api/posts/user/{userId}");
        var commentsTask = _httpClient.GetAsync($"https://comment-service/api/comments/user/{userId}");

        await Task.WhenAll(postsTask, commentsTask);
        var posts = await postsTask;
        var comments = await commentsTask;

        var feed = FeedBuilder.BuildFeed(posts, comments);

        return Ok(feed);
    }

    [HttpPost("update")]
    public async Task CreateOrUpdateFeed(Dto dto)
    {
        await feedService.CreateOrUpdateFeed(dto);

    }
}
