using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/[controller]")]
public class FeedController(HttpClient _httpClient) : ControllerBase
{
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetFeedV1(string userId)
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


    [HttpGet]
    public async Task GetFeedV2(Dto dto)
    {
        var feed = await feedService.GetFeed(dto);
        return Ok(feed);
    }
}
