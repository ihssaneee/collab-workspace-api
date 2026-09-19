using CollabWorkspace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CollabWorkspace.Infrastructure.Identity;

namespace CollabWorkspace.Infrastructure.Configurations
{
    public class DocumentConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.HasKey(document => document.Id);

            builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey( document => document.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne<Workspace>()
            .WithMany()
            .HasForeignKey(document => document.WorkspaceId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}