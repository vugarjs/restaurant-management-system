using Microsoft.EntityFrameworkCore;
using RestorantApp.Entity.Entities;
using RestorantApp.Entity.Entities.Common;

namespace RestorantApp.DataAccess.Context;

public class RestorantContext : DbContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var stringConnection = "Data Source=localhost\\SQLEXPRESS;Database=RestorantDB;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False;Command Timeout=30";
        optionsBuilder.UseSqlServer(stringConnection);
        base.OnConfiguring(optionsBuilder);

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RestorantContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<AuditAble>()
                        .Where(e => e.State == EntityState.Added);


        foreach (var item in entries)
        {
            switch (item.State)
            {
                case EntityState.Added:
                    item.Entity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    item.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }

        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}
