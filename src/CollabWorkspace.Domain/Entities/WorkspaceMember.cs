using CollabWorkspace.Domain.Enums;
namespace CollabWorkspace.Domain.Entities
{
    public class WorkspaceMember
    {
        public Guid Id { get; set; }

        public Guid WorkspaceId { get; set; }

        public Guid UserId { get; set; }

        public WorkspaceRole Role { get; set; } 

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}