using CollabWorkspace.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CollabWorkspace.Infrastructure.Data
{
   public class CollabWorkspaceDbContext : DbContext
    {
        public CollabWorkspaceDbContext(DbContextOptions<CollabWorkspaceDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
    }
}