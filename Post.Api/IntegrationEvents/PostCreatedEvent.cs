// A common mistake is to put this an a shared class library.
// That violates service autonomy, but this causes duplication


namespace Comment.API.IntegrationEvents
{
    public record PostCreatedEvent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Content { get; set; }
        public Guid PostId { get; set; }
        public string EventType { get; set; } = "PostCreated";
        public Guid UserId { get; set; }
        public string? Type { get; set; } = "Post";
    }
}
