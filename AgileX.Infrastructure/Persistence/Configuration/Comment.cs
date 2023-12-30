using AgileX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgileX.Infrastructure.Persistence.Configuration;

public class CommentConfigurations : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("comments");

        builder.HasKey(comment => comment.CommentId);
        builder.Property(comment => comment.CommentId).ValueGeneratedNever().HasColumnName("id");

        builder.Property(comment => comment.TicketId).IsRequired().HasColumnName("ticket_id");
        builder.Property(comment => comment.ParentCommentId).HasColumnName("parent_comment_id");
        builder.Property(comment => comment.CommentedBy).IsRequired().HasColumnName("commented_by");
        builder.Property(comment => comment.Text).IsRequired().HasColumnName("text");
        builder.Property(comment => comment.AttachedCode).HasColumnName("attached_code");
        builder.Property(comment => comment.SubCommentsCount).HasColumnName("sub_comments_count");

        builder
            .Property(comment => comment.IsDeleted)
            .HasDefaultValue(false)
            .HasColumnName("is_deleted");

        builder.Property(comment => comment.CreatedAt).IsRequired().HasColumnName("created_at");
        builder.Property(comment => comment.UpdatedAt).IsRequired().HasColumnName("updated_at");
        builder.Property(comment => comment.DeletedAt).HasColumnName("deleted_at");
    }
}
