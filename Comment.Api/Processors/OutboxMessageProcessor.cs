using MassTransit;
using System.Text.Json;

namespace Post.API.Processors;
public class OutboxProcessor : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IPublishEndpoint _publishEndpoint;

    public OutboxProcessor(IServiceProvider serviceProvider, IPublishEndpoint publishEndpoint)
    {
        _serviceProvider = serviceProvider;
        _publishEndpoint = publishEndpoint;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            using var transaction = dbContext.Database.BeginTransactionAsync();


            var pendingMessages = await dbContext.OutboxMessages
                .OrderBy(m => m.CreatedAt)
                .Take(100)
                .ToListAsync(stoppingToken);

            foreach (var message in pendingMessages)
            {
                var payload = JsonSerializer.Serialize(message.Payload);

                await _publishEndpoint.Publish(payload);

                // Remove the message from the outbox after successful publish
                // Or you could update the status to processed if you wish
                message.IsProcessed = true;
                await dbContext.SaveChangesAsync(stoppingToken);
                await transaction.CommitAsync();
            }
        }

        // Wait for a short interval before checking again
        await Task.Delay(10000, stoppingToken);

    }
}
