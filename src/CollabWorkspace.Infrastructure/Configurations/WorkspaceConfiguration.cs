using CollabWorkspace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CollabWorkspace.Infrastructure.Identity;

namespace CollabWorkspace.Infrastructure.Configurations
{
    public class WorkspaceConfiguration : IEntityTypeConfiguration<Workspace>
    {
        public void Configure(EntityTypeBuilder<Workspace> builder)
        {
            builder.HasKey(workspace => workspace.Id);

            builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(workspace => workspace.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}