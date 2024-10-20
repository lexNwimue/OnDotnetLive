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
        var repliesTask = _httpClient.GetAsync($"https://reply-service/api/replies/user/{userId}");

        await Task.WhenAll(postsTask, commentsTask, repliesTask);
        var posts = await postsTask;
        var comments = await commentsTask;
        var replies = await repliesTask;

        var feed = FeedBuilder.BuildFeed(posts, comments, replies);

        return Ok(feed);
    }


}
