using AgileX.Domain.Entities;
using AgileX.Domain.ObjectValues;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgileX.Infrastructure.Persistence.Configuration;

public class MemberConfigurations : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.ToTable("members");

        builder.Property(member => member.UserId).IsRequired().HasColumnName("user_id");
        builder.Property(member => member.ProjectId).IsRequired().HasColumnName("project_id");
        builder.HasKey(member => new { member.UserId, member.ProjectId });

        builder
            .Property(member => member.Membership)
            .IsRequired()
            .HasColumnName("membership")
            .HasConversion(
                value => ConvertMembershipToString(value),
                value => ConvertStringToMembership(value)
            );

        builder
            .Property(member => member.IsDeleted)
            .HasDefaultValue(false)
            .HasColumnName("is_deleted");

        builder.Property(member => member.CreatedAt).IsRequired().HasColumnName("created_at");
        builder.Property(member => member.DeletedAt).HasColumnName("deleted_at");
    }

    private static string ConvertMembershipToString(Membership membership)
    {
        return membership switch
        {
            Membership.PROJECT_OWNER => "project_owner",
            Membership.PROJECT_MEMBER => "project_member",
            _ => "project_member"
        };
    }

    private static Membership ConvertStringToMembership(string rawMembership)
    {
        return rawMembership switch
        {
            "project_owner" => Membership.PROJECT_OWNER,
            "project_member" => Membership.PROJECT_MEMBER,
            _ => Membership.PROJECT_MEMBER
        };
    }
}
