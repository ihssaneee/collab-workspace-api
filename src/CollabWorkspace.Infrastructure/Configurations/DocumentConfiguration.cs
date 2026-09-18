using CollabWorkspace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollabWorkspace.Infrastructure.Configurations
{
    public class DocumentConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.HasKey(document => document.Id);

            builder.HasOne<User>()
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