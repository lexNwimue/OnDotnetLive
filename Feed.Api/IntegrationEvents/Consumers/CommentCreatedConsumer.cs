using Comment.API.IntegrationEvents;
using MassTransit;

namespace Newsfeed.API.IntegrationEvents.Consumers
{
    public class CommentCreatedConsumer : IConsumer<CommentCreatedEvent>
    {
        public async Task Consume(ConsumeContext<CommentCreatedEvent> context)
        {
            // Process event as needed
            var dto = context.Message;
            await feedService.ProcessFeed(dto);
        }
    }
}
