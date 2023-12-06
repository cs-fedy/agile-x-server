using AgileX.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AgileX.Infrastructure.Persistence;

public class CustomDbContext : DbContext
{
    public CustomDbContext(DbContextOptions<CustomDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
