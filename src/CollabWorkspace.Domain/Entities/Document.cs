namespace CollabWorkspace.Domain.Entities
{
    public class Document
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public Guid WorkspaceId { get; set; }

        public Guid OwnerId { get; set; }

        public int Version { get; set; } = 1;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? DeletedAt { get; set; }
    }
}