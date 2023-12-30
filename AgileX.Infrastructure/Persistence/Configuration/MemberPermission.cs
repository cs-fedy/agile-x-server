using AgileX.Domain.Entities;
using AgileX.Domain.ObjectValues;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgileX.Infrastructure.Persistence.Configuration;

public class MemberPermissionConfigurations : IEntityTypeConfiguration<MemberPermission>
{
    public void Configure(EntityTypeBuilder<MemberPermission> builder)
    {
        builder.ToTable("member_permissions");

        builder.Property(permission => permission.UserId).IsRequired().HasColumnName("user_id");

        builder
            .Property(permission => permission.ProjectId)
            .IsRequired()
            .HasColumnName("project_id");

        builder.HasKey(permission => new { permission.UserId, permission.ProjectId });

        builder.Property(permission => permission.Name).IsRequired().HasColumnName("name");

        builder
            .Property(permission => permission.Description)
            .IsRequired()
            .HasColumnName("description");

        builder
            .Property(permission => permission.Permission)
            .IsRequired()
            .HasColumnName("permission")
            .HasConversion(
                value => ConvertPermissionToString(value),
                value => ConvertStringToPermission(value)
            );

        builder
            .Property(permission => permission.Entity)
            .IsRequired()
            .HasColumnName("entity")
            .HasConversion(
                value => ConvertEntityToString(value),
                value => ConvertStringToEntity(value)
            );

        builder
            .Property(permission => permission.IsDeleted)
            .HasDefaultValue(false)
            .HasColumnName("is_deleted");

        builder
            .Property(permission => permission.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");

        builder.Property(permission => permission.DeletedAt).HasColumnName("deleted_at");
    }

    private static string ConvertEntityToString(Entity entity)
    {
        return entity switch
        {
            Entity.Project => "project",
            Entity.Permission => "permission",
            Entity.Attachment => "attachment",
            Entity.Comment => "comment",
            Entity.Dependency => "dependency",
            Entity.Label => "label",
            Entity.Member => "member",
            Entity.Sprint => "sprint",
            Entity.Ticket => "ticket",
            Entity.User => "user",
            _ => throw new ArgumentOutOfRangeException(nameof(entity), entity, null)
        };
    }

    private static Entity ConvertStringToEntity(string rawEntity)
    {
        return rawEntity switch
        {
            "project" => Entity.Project,
            "permission" => Entity.Permission,
            "attachment" => Entity.Attachment,
            "comment" => Entity.Comment,
            "dependency" => Entity.Dependency,
            "label" => Entity.Label,
            "member" => Entity.Member,
            "sprint" => Entity.Sprint,
            "ticket" => Entity.Ticket,
            "user" => Entity.User,
            _ => throw new ArgumentOutOfRangeException(nameof(rawEntity), rawEntity, null)
        };
    }

    private static string ConvertPermissionToString(Permission permission)
    {
        return permission switch
        {
            Permission.ListMemberPermissions => "list_member_permission",
            Permission.GrantPermission => "grant_permission",
            Permission.DeleteProject => "delete_project",
            Permission.UpdateProject => "update_project",
            Permission.UpdateProjectDeadline => "update_project_deadline",
            Permission.AddMember => "add_member",
            Permission.CreateSprint => "create_sprint",
            Permission.UpdateSprint => "update_sprint",
            Permission.DeleteSprint => "delete_sprint",
            Permission.CreateTicket => "create_ticket",
            Permission.ChangeTicketSprint => "change_ticket_sprint",
            Permission.ChangeTicketDeadline => "change_ticket_deadline",
            Permission.DeleteTicket => "delete_ticket",
            Permission.ChangeSprintDates => "change_sprint_dates",
            Permission.AddDependency => "add_dependency",
            Permission.DeleteDependency => "delete_dependency",
            Permission.CreateLabel => "create_label",
            Permission.DeleteLabel => "delete_label",
            _ => throw new ArgumentOutOfRangeException(nameof(permission), permission, null)
        };
    }

    private static Permission ConvertStringToPermission(string rawPermission)
    {
        return rawPermission switch
        {
            "list_member_permission" => Permission.ListMemberPermissions,
            "grant_permission" => Permission.GrantPermission,
            "delete_project" => Permission.DeleteProject,
            "update_project" => Permission.UpdateProject,
            "update_project_deadline" => Permission.UpdateProjectDeadline,
            "add_member" => Permission.AddMember,
            "create_sprint" => Permission.CreateSprint,
            "update_sprint" => Permission.UpdateSprint,
            "delete_sprint" => Permission.DeleteSprint,
            "create_ticket" => Permission.CreateTicket,
            "change_ticket_sprint" => Permission.ChangeTicketSprint,
            "change_ticket_deadline" => Permission.ChangeTicketDeadline,
            "delete_ticket" => Permission.DeleteTicket,
            "change_sprint_dates" => Permission.ChangeSprintDates,
            "add_dependency" => Permission.AddDependency,
            "delete_dependency" => Permission.DeleteDependency,
            "create_label" => Permission.CreateLabel,
            "delete_label" => Permission.DeleteLabel,
            _ => throw new ArgumentOutOfRangeException(nameof(rawPermission), rawPermission, null)
        };
    }
}
