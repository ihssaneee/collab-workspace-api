using CollabWorkspace.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CollabWorkspace.Infrastructure.Data
{
   public class CollabWorkspaceDbContext : DbContext
    {
        public CollabWorkspaceDbContext(DbContextOptions<CollabWorkspaceDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CollabWorkspaceDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<User> Users => Set<User>();
        public DbSet<Workspace> Workspaces => Set<Workspace>();
        public DbSet<WorkspaceMember> WorkspaceMembers => Set<WorkspaceMember>();
        public DbSet<Document> Documents => Set<Document>();
        public DbSet<DocumentBlock> DocumentBlocks => Set<DocumentBlock>();
    }
}