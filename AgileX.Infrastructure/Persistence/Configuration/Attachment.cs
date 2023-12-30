using AgileX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgileX.Infrastructure.Persistence.Configuration;

public class AttachmentConfigurations : IEntityTypeConfiguration<Attachment>
{
    public void Configure(EntityTypeBuilder<Attachment> builder)
    {
        builder.ToTable("attachments");

        builder.HasKey(attachment => attachment.AttachmentId);
        builder
            .Property(attachment => attachment.AttachmentId)
            .ValueGeneratedNever()
            .HasColumnName("id");

        builder.Property(attachment => attachment.TicketId).IsRequired().HasColumnName("ticket_id");
        builder.Property(attachment => attachment.Url).IsRequired().HasColumnName("url");
        builder.Property(attachment => attachment.Type).IsRequired().HasColumnName("type");

        builder
            .Property(attachment => attachment.IsDeleted)
            .HasDefaultValue(false)
            .HasColumnName("is_deleted");

        builder
            .Property(attachment => attachment.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");

        builder.Property(attachment => attachment.DeletedAt).HasColumnName("deleted_at");
    }
}
