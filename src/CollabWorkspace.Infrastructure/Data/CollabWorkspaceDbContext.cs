using CollabWorkspace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using CollabWorkspace.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CollabWorkspace.Infrastructure.Data
{
   public class CollabWorkspaceDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public CollabWorkspaceDbContext(DbContextOptions<CollabWorkspaceDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CollabWorkspaceDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Workspace> Workspaces => Set<Workspace>();
        public DbSet<WorkspaceMember> WorkspaceMembers => Set<WorkspaceMember>();
        public DbSet<Document> Documents => Set<Document>();
        public DbSet<DocumentBlock> DocumentBlocks => Set<DocumentBlock>();
    }
}