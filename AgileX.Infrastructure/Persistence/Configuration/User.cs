using AgileX.Domain.Entities;
using AgileX.Domain.ObjectValues;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgileX.Infrastructure.Persistence.Configuration;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(user => user.UserId);
        builder.Property(user => user.UserId).ValueGeneratedNever().HasColumnName("id");

        builder.Property(user => user.Email).IsRequired().HasColumnName("email");
        builder.Property(user => user.Password).IsRequired().HasColumnName("password");
        builder.Property(user => user.FullName).IsRequired().HasColumnName("full_name");
        builder.Property(user => user.Username).IsRequired().HasColumnName("username");

        builder
            .Property(user => user.Role)
            .IsRequired()
            .HasColumnName("role")
            .HasConversion(
                value => value == Role.USER ? "user" : "admin",
                value => value == "user" ? Role.USER : Role.ADMIN
            );

        builder
            .Property(user => user.IsConfirmed)
            .HasDefaultValue(false)
            .HasColumnName("is_confirmed");

        builder.Property(user => user.IsDeleted).HasDefaultValue(false).HasColumnName("is_deleted");

        builder.Property(user => user.CreatedAt).IsRequired().HasColumnName("created_at");
        builder.Property(user => user.UpdatedAt).IsRequired().HasColumnName("updated_at");
        builder.Property(user => user.DeletedAt).HasColumnName("deleted_at");
    }
}
