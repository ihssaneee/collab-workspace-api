using CollabWorkspace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CollabWorkspace.Infrastructure.Identity;

namespace CollabWorkspace.Infrastructure.Configurations
{
    public class WorkspaceMemberConfiguration : IEntityTypeConfiguration<WorkspaceMember>
    {
         public void Configure(EntityTypeBuilder<WorkspaceMember> builder)
        {
            builder.HasKey(workspaceMember => workspaceMember.Id);


            builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey( workspaceMember => workspaceMember.UserId)
            .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne<Workspace>()
            .WithMany()
            .HasForeignKey( workspaceMember => workspaceMember.WorkspaceId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}