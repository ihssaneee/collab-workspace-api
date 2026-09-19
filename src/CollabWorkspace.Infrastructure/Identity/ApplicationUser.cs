using Microsoft.AspNetCore.Identity;

namespace CollabWorkspace.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}