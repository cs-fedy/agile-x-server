using AgileX.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AgileX.Infrastructure.Persistence;

public class CustomDbContext : DbContext
{
    public CustomDbContext(DbContextOptions<CustomDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Refresh> Refreshes { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<MemberPermission> MemberPermissions { get; set; }
    public DbSet<Sprint> Sprints { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Dependency> Dependencies { get; set; }
    public DbSet<Attachment> Attachments { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Label> Labels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
