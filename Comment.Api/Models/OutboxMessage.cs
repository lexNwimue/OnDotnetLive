namespace Post.Api.Models
{
    public class OutboxMessage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public dynamic Payload { get; set; }
        public bool IsProcessed { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
