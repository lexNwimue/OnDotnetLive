using Comment.API.IntegrationEvents;
using MassTransit;

namespace Newsfeed.API.IntegrationEvents.Consumers
{
    public class PostCreatedConsumer : IConsumer<PostCreatedEvent>
    {
        private readonly AppDbContext _dbContext;

        public PostCreatedConsumer(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<PostCreatedEvent> consumerContext)
        {
            var payload = consumerContext.Message;

            // Implement Idempotency??
            var existingPost = await _dbContext.Feed.Find(feed => feed.PostId == payload.PostId);
            if (existingPost != null)
            {
                Console.WriteLine($"Post {existingPost.PostId} already processed");
                return;
            }

            // Process event as needed
            var dto = context.Message;
            await feedService.ProcessFeed(dto);
        }
    }
}
