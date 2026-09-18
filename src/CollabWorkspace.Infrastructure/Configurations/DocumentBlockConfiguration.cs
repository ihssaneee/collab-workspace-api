using CollabWorkspace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollabWorkspace.Infrastructure.Configurations
{
    public class DocumentBlockConfiguration : IEntityTypeConfiguration<DocumentBlock>
    {
        public void Configure(EntityTypeBuilder<DocumentBlock> builder)
        {
            builder.HasKey(documentBlock => documentBlock.Id);

            builder.HasOne<Document>()
            .WithMany()
            .HasForeignKey(documentBlock => documentBlock.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Property(documentBlock => documentBlock.Content)
            .HasColumnType("jsonb");

        }   
    } 
}