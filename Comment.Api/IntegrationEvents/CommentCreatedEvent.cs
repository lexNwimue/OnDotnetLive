// A common mistake is to put this an a shared class library.
// That violates service autonomy, but this causes duplication


namespace Comment.API.IntegrationEvents
{
    public record CommentCreatedEvent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CommentId { get; set; }
        public string Content { get; set; }
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public string? Type { get; set; } = "Comment";
    }
}
