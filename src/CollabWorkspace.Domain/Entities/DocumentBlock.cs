namespace CollabWorkspace.Domain.Entities
{
    public class DocumentBlock
    {
        public Guid Id { get; set; }

        public Guid DocumentId { get; set; }

        public int Position { get; set; }

        public string Type { get; set; } = string.Empty;

        public string Content { get; set; } = "{}";
    }
}